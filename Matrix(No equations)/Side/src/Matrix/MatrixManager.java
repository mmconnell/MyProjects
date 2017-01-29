package Matrix;

import Fraction.Fraction;
import Input.InputManagement;
import Input.RegexManagement;

import java.io.*;
import java.util.*;

public class MatrixManager
{
   private List<Matrix> matrices;
   private Matrix currentMatrix;
   private int curIndex;
   private InputManagement inputManagement;
   private List<Stack<String>> operations;
   private List<ArrayList<String>> forwardOperations;

   private String unrecognizedCommandError;
   private String outOfBoundsError;
   private String matrixSizeError;

   public MatrixManager()
   {
      unrecognizedCommandError = "";
      outOfBoundsError = " is out of bounds";
      matrixSizeError = "A matrix cannot have a different number of elements in each row";
      matrices = new ArrayList<>();
      currentMatrix = null;
      inputManagement = new InputManagement();
      operations = new ArrayList<Stack<String>>();
      forwardOperations = new ArrayList<ArrayList<String>>();
   }

   public boolean command(String str)
   {
      String command;

      while (str.length() > 1)
      {
         if (!str.contains(";"))
         {
            command = str;
            str = "";
         } else
         {
            command = str.substring(0, str.indexOf(';'));
            str = str.substring(str.indexOf(';') + 1);
         }
         unrecognizedCommandError = command + " could not be interpreted as a command";
         command = command.replaceAll(" ", "");
         command = command.toLowerCase();

         if (!command.isEmpty() && !interpretCommand(command))
         {
            return false;
         }

      }
      return true;
   }

   private boolean interpretCommand(String str)
   {
      RegexManagement commmandToExecute = inputManagement.InterpretCommand(str);
      switch(commmandToExecute)
      {
         case Print:
            System.out.print(currentMatrix.printRows());
            break;
         case Echelon:
            currentMatrix.rowEchelon(operations.get(curIndex),forwardOperations.get(curIndex));
            break;
         case Exit:
            return false;
         case ViewMatrices:
            printAll();
            break;
         case Help:
            printHelp();
            break;
         case Whole:
            makeWhole(str);
            break;
         case AddMatrices:
            addTwoMatrices(str);
            break;
         case SubtractMatrices:
            subtractTwoMatrices(str);
            break;
         case MultiplyMatrices:
            multiplyMatrix(str);
            break;
         case ScaleMatrix:
            scaleMatrix(str);
            break;
         case RemoveMatrices:
            removeMatrix(str);
            break;
         case ChangeCurrent:
            changeMatrix(str);
            break;
         case NewMatrix:
            addMatrix(str);
            break;
         case SwapRows:
            swapRows(str);
            break;
         case SubtractRows:
            subtractTwoRows(str);
            break;
         case AddRows:
            addTwoRows(str);
            break;
         case ScaleRow:
            scaleRow(str);
            break;
         case PrintAllOperations:
            printOperations();
            break;
         case Undo:
            undo();
            break;
         default:
            printError();
            return true;
      }
      return true;
   }

   private void printError()
   {
      System.out.println(this.unrecognizedCommandError);
   }

   private void printOutOfBoundsError(String str)
   {
      System.out.println(str + this.outOfBoundsError);
   }

   private void printMatrixSizeError()
   {
      System.out.println(this.matrixSizeError);
   }

   private boolean printOperations()
   {
      if(currentMatrix == null)
      {
         return true;
      }

      ArrayList<String> list = forwardOperations.get(curIndex);
      Stack<String> prevOps = operations.get(curIndex);
      Stack<String> tempStack = new Stack<>();
      String temp, toPrint = "", str;
      currentMatrix.stopStack();
      for(int x = list.size() - 1; x > -1; x--)
      {
         temp = prevOps.pop();
         tempStack.push(temp);
         str = list.get(x);
         if(!str.equals("stop") && !str.equals("start"))
         {
            toPrint = currentMatrix.printRows() + toPrint;
            toPrint = str + "\n" + toPrint;
            interpretCommand(temp);
         }
      }

      System.out.println(currentMatrix.printRows() + toPrint);
      currentMatrix.forceForward();
      String stuffToPrint = "";
      for(String command : list)
      {
         stuffToPrint += command + ";";
         if(!command.equals("stop") && !command.equals("start"))
         {
            interpretCommand(command);
         }
         prevOps.push(tempStack.pop());
      }
      //System.out.println(stuffToPrint);
      currentMatrix.releaseForward();
      currentMatrix.startStack();
      return true;
   }

   private boolean scaleRow(String str)
   {
      String fracStr = str.substring(0, str.indexOf('r'));
      String rowStr1 = str.substring(str.indexOf('r') + 1);

      Fraction fracOne = inputManagement.findFraction(fracStr);

      try
      {
         currentMatrix.scalarOperation(fracOne, Integer.parseInt(rowStr1) - 1, operations.get(curIndex), forwardOperations.get(curIndex));
      }
      catch (IllegalArgumentException e)
      {
         System.out.println(e.getMessage());
      }
      return true;
   }

   private boolean subtractTwoRows(String str)
   {
      String scale1 = str.substring(0, str.indexOf('r'));
      String row1 = str.substring(str.indexOf('r') + 1, str.indexOf('-'));
      String scale2 = str.substring(str.indexOf('-') + 1, str.indexOf('r',str.indexOf('-')));
      String row2 = str.substring(str.lastIndexOf('r') + 1);

      Fraction frac1 = inputManagement.findFraction(scale1);
      Fraction frac2 = inputManagement.findFraction(scale2);
      frac2.multiply(inputManagement.findFraction("-1"));

      return addTwoMatrices(frac1 + "r" + row1 + "+" + frac2 + "r" + row2);
   }

   private boolean addTwoRows(String str)
   {
      String scale1 = str.substring(0, str.indexOf('r'));
      String row1 = str.substring(str.indexOf('r') + 1, str.indexOf('+'));
      String scale2 = str.substring(str.indexOf('+') + 1, str.indexOf('r',str.indexOf('+')));
      String row2 = str.substring(str.lastIndexOf('r') + 1);

      Fraction frac1 = inputManagement.findFraction(scale1);
      Fraction frac2 = inputManagement.findFraction(scale2);

      try
      {
         currentMatrix.addOperation(frac1, Integer.parseInt(row1) - 1, frac2, Integer.parseInt(row2) - 1, operations.get(curIndex), forwardOperations.get(curIndex));
      }
      catch (IllegalArgumentException e)
      {
         System.out.println(e.getMessage());
      }
      return true;
   }

   private boolean swapRows(String str)
   {
      String num1 = str.substring(1,str.indexOf('<'));
      String num2 = str.substring(str.indexOf('>') + 2);

      try
      {
         currentMatrix.swapOperation(Integer.parseInt(num1) - 1, Integer.parseInt(num2) - 1, operations.get(curIndex), forwardOperations.get(curIndex));
      }
      catch(IllegalArgumentException e)
      {
         System.out.println(e.getMessage());
      }
      return true;
   }

   private boolean makeWhole(String str)
   {
      int row = Integer.parseInt(str.substring(str.indexOf('r') + 1));

      try
      {
         currentMatrix.makeWhole(row -1, operations.get(curIndex), forwardOperations.get(curIndex));
      }
      catch(IllegalArgumentException e)
      {
         System.out.println(e.getMessage());
      }
      return true;
   }

   private boolean undo()
   {
      Stack<String> myStack = operations.get(curIndex);
      ArrayList<String> forward = forwardOperations.get(curIndex);
      String operation;
      String command = "";
      if(myStack.size() > 0)
      {
         operation = myStack.pop();
         forward.remove(forward.size() - 1);
         if(operation.equals("start"))
         {
            while(!operation.equals("stop"))
            {
               operation = myStack.pop();
               forward.remove(forward.size() - 1);
               if(!operation.equals("stop"))
               {
                  command = command + operation + ";";
               }
            }
         }
         else
         {
            command = operation;
         }
         currentMatrix.stopStack();
         command(command);
         currentMatrix.startStack();
      }
      return true;
   }


   private boolean removeMatrix(String str)
   {
      int val = Integer.parseInt(str.substring(str.indexOf('m') + 1));

      if(val < 1 || val > matrices.size())
      {
         printOutOfBoundsError(val + "");
         return true;
      }

      Matrix remove = matrices.remove(val - 1);
      operations.remove(val -1);
      forwardOperations.remove(val - 1);
      if(currentMatrix == remove)
      {
         if(matrices.size() == 0)
         {
            currentMatrix = null;
         }
         else
         {
            currentMatrix = matrices.get(0);
            curIndex = 0;
         }
      }

      return true;
   }

   private boolean changeMatrix(String str)
   {
      int val = Integer.parseInt(str.substring(str.indexOf('m') + 1));

      if(val < 1 || val > matrices.size())
      {
         printOutOfBoundsError(val + "");
         return true;
      }

      currentMatrix = matrices.get(val - 1);
      curIndex = val -1;
      return true;
   }

   private void printAll()
   {
      int count = 1;
      for(Matrix matrix: matrices)
      {
         System.out.println(count + ") ");
         System.out.println(matrix.printRows());
         count++;
      }
   }

   private boolean scaleMatrix(String str)
   {
      int val = Integer.parseInt(str.substring(str.indexOf('m') + 1));

      Fraction fraction = inputManagement.findFraction(str.substring(0, str.indexOf('m')));

      if(val < 1 || val > matrices.size())
      {
         printOutOfBoundsError(val + "");
      }

      Matrix matrix = matrices.get(val - 1);

      try
      {
         matrix.scaleMatrix(fraction, operations.get(matrices.indexOf(matrix)), forwardOperations.get(matrices.indexOf(matrix)));
      }
      catch(IllegalArgumentException e)
      {
         System.out.println(e.getMessage());
      }

      return true;
   }

   private boolean multiplyMatrix(String str)
   {
      int val1 = Integer.parseInt(str.substring(str.indexOf('m') + 1, str.indexOf('x')));
      int val2 = Integer.parseInt(str.substring(str.lastIndexOf('m') + 1));

      Fraction scale1 = inputManagement.findFraction(str.substring(0,str.indexOf('m')));
      Fraction scale2 = inputManagement.findFraction(str.substring(str.indexOf('x')+1, str.lastIndexOf('m')));

      if(val1 < 1 || val1 > matrices.size())
      {
         printOutOfBoundsError(val1 + "");
         return true;
      }

      Matrix matrix1 = matrices.get(val1 - 1);

      if(val2 < 1 || val2 > matrices.size())
      {
         printOutOfBoundsError(val2 + "");
         return true;
      }

      Matrix matrix2 = matrices.get(val2 - 1);

      if(matrix1.getColumns() != matrix2.getRows())
      {
         System.out.println("These two Matrices could not be multiplied together");
         return true;
      }

      Matrix tempMat1, tempMat2;

      try
      {
         tempMat1 = matrix1.clone();
         tempMat2 = matrix2.clone();
         tempMat1.scaleMatrix(scale1, new Stack<>(), new ArrayList<>());
         tempMat2.scaleMatrix(scale2, new Stack<>(), new ArrayList<>());
         matrices.add(Matrix.multiply(tempMat1, tempMat2));
         operations.add(new Stack<String>());
         forwardOperations.add(new ArrayList<>());
         currentMatrix = matrices.get(matrices.size() -1);
         curIndex = matrices.size() - 1;
         interpretCommand("print");
      }
      catch(IllegalArgumentException e)
      {
         System.out.println(e.getMessage());
      }

      return true;
   }

   private boolean subtractTwoMatrices(String str)
   {
      int val1 = Integer.parseInt(str.substring(str.indexOf('m') + 1, str.indexOf('-')));
      int val2 = Integer.parseInt(str.substring(str.lastIndexOf('m') + 1));

      Fraction scale1 = inputManagement.findFraction(str.substring(0,str.indexOf('m')));
      Fraction scale2 = inputManagement.findFraction(str.substring(str.indexOf('-')+1, str.lastIndexOf('m')));
      scale2.multiply(inputManagement.findFraction("-1"));

      return addTwoMatrices(scale1 + "m" + val1 + "+" + scale2 + "m" + val2);
   }

   private boolean addTwoMatrices(String str)
   {
      int val1 = Integer.parseInt(str.substring(str.indexOf('m') + 1, str.indexOf('+')));
      int val2 = Integer.parseInt(str.substring(str.lastIndexOf('m') + 1));

      Fraction scale1 = inputManagement.findFraction(str.substring(0,str.indexOf('m')));
      Fraction scale2 = inputManagement.findFraction(str.substring(str.indexOf('+')+1, str.lastIndexOf('m')));

      if(val1 < 1 || val1 > matrices.size())
      {
         printOutOfBoundsError(val1 + "");
         return true;
      }

      Matrix matrix1 = matrices.get(val1 - 1);

      if(val2 < 1 || val2 > matrices.size())
      {
         printOutOfBoundsError(val2 + "");
         return true;
      }

      Matrix matrix2 = matrices.get(val2 - 1);

      if(matrix1.getColumns() != matrix2.getColumns() || matrix1.getRows() != matrix2.getRows())
      {
         System.out.println("You can only add or subtract two matrices that are the same dimensions");
         return true;
      }

      Matrix tempMat1, tempMat2;

      try
      {
         tempMat1 = matrix1.clone();
         tempMat2 = matrix2.clone();
         tempMat1.scaleMatrix(scale1, new Stack<>(), new ArrayList<>());
         tempMat2.scaleMatrix(scale2, new Stack<>(), new ArrayList<>());
         matrices.add(Matrix.addMatrix(tempMat1, tempMat2));
         operations.add(new Stack<String>());
         forwardOperations.add(new ArrayList<>());
         currentMatrix = matrices.get(matrices.size() - 1);
         curIndex = matrices.size() - 1;
         interpretCommand("print");
      }
      catch (IllegalArgumentException e)
      {
         System.out.println(e.getMessage());
      }

      return true;
   }

   private boolean addMatrix(String str)
   {
      int count = 0, tempCount = 0;


      str = str.substring(str.indexOf('[') + 1, str.lastIndexOf(']'));

      Scanner kb = new Scanner(str);
      kb.useDelimiter(":");

      List<String> strings = new ArrayList<>();

      while(kb.hasNext())
      {
         strings.add(kb.next());
      }

      if(strings.size() < 1)
      {
         printError();
         return true;
      }

      String testString = strings.get(0);

      count = count(testString, ',');

      for(String rows: strings)
      {
         if(count(rows, ',') != count)
         {
            printMatrixSizeError();
            return true;
         }
      }

      Matrix matrix = new Matrix(count + 1);
      List<Fraction> list;

      for(String rows: strings)
      {
         tempCount = 0;
         list = new ArrayList<>();
         kb = new Scanner(rows);
         kb.useDelimiter(",");

         while(kb.hasNext())
         {
            list.add(inputManagement.findFraction(kb.next()));
         }

         matrix.addRow(list);
      }

      matrices.add(matrix);
      operations.add(new Stack<String>());
      forwardOperations.add(new ArrayList<>());
      currentMatrix = matrix;
      curIndex = matrices.indexOf(currentMatrix);

      return true;
   }

   private void printHelp()
   {
      File file = new File("help.txt");
      Scanner fin;
      try
      {
         fin = new Scanner(file);
      }catch(FileNotFoundException e)
      {
         System.out.println(e.getMessage());
         return;
      }

      while(fin.hasNext())
      {
         System.out.println(fin.nextLine());
      }
   }

   private int count(String str, char c)
   {
      int count = 0;
      for(int x = 0; x < str.length(); x++)
      {
         if(str.charAt(x) == c)
         {
            count++;
         }
      }
      return count;
   }
}