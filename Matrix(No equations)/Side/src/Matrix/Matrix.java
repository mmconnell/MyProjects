package Matrix;

import Fraction.Fraction;
import java.util.*;

public class Matrix implements Cloneable
{
   private List<List<Fraction>> manageRows;
   private int columns;
   private boolean addToStack;
   private boolean forceForward;

   public Matrix(int columns)
   {
      manageRows = new ArrayList<List<Fraction>>();
      this.columns = columns;
      addToStack = true;
      forceForward = false;
   }

   public void addRow(List<Fraction> addIn)
   {
      if(addIn.size() != columns)
      {
         //System.out.println("Bad parameters");
         throw new IllegalArgumentException("Bad parameters");
      }
      manageRows.add(addIn);
   }

   public String printRows()
   {
      String str = "", temp;
      int count = 1;
      int numOfSpaces = 1;
      for(List<Fraction> list: manageRows)
      {
         for(Fraction num: list)
         {
            numOfSpaces = Math.max(numOfSpaces, num.toString().length());
         }
      }
      for(List<Fraction> list: manageRows)
      {
         str += "R" + count + ": ";
         for(Fraction num: list)
         {
            temp = num.toString();
            while(temp.length() < numOfSpaces)
            {
               temp = " " + temp;
            }
            str += temp + " ";
         }
         str += "\n";
         count++;
      }
      return str;
   }

   public void addOperation(Fraction multipleA, int rowA, Fraction multipleB, int rowB, Stack<String> operations, ArrayList<String> forwardOperations)
   {
      if(rowA >= manageRows.size() || rowB >= manageRows.size() || rowA < 0 || rowB < 0)
      {
         throw new IllegalArgumentException("Bad parameters");
         //System.out.println("Bad parameters");
      }

      if(rowA == rowB)
      {
         throw new IllegalArgumentException("Cannot add a row to itself. Please see the scalar operation");
      }

      if(addToStack)
      {
         operations.push(Fraction.negate(multipleA) + "r" + (rowA + 1) + "+" + Fraction.reciprocal(multipleB) + "r" + (rowB + 1));
         forwardOperations.add(multipleA + "r" + (rowA+1) + "+" + multipleB + "r" + (rowB + 1));
      }

      List<Fraction> listA = manageRows.get(rowA), listB = manageRows.get(rowB);
      Fraction val;
      for(int x = 0; x < listA.size(); x++)
      {
         if(addToStack || forceForward)
         {
            listB.get(x).multiply(multipleB);
            listA.get(x).multiply(multipleA);
            listB.get(x).add(listA.get(x));
            listA.get(x).divide(multipleA);
         }
         else
         {
            listA.get(x).multiply(multipleA);
            listB.get(x).add(listA.get(x));
            listA.get(x).divide(multipleA);
            listB.get(x).multiply(multipleB);
         }
      }
   }

   public void swapOperation(int rowA, int rowB, Stack<String> operations, ArrayList<String> forwardOperations)
   {
      if(rowA >= manageRows.size() || rowB >= manageRows.size() || rowA < 0 || rowB < 0 || rowA == rowB)
      {
         //System.out.println("Bad parameters");
         throw new IllegalArgumentException("Bad parameters");
      }

      if(addToStack)
      {
         operations.push("r" + (rowA + 1) + "<>" + "r" + (rowB + 1));
         forwardOperations.add("r" + (rowA + 1) + "<>" + "r" + (rowB + 1));
      }

      if(rowA < rowB)
      {
         rowA = rowA^rowB;
         rowB = rowA^rowB;
         rowA = rowA^rowB;
      }
      List<Fraction> listA = manageRows.remove(rowA);
      List<Fraction> listB = manageRows.remove(rowB);
      manageRows.add(rowB, listA);
      manageRows.add(rowA, listB);
   }

   public void scaleMatrix(Fraction fraction, Stack<String> operations, ArrayList<String> forwardOperations)
   {
      if(addToStack)
      {
         operations.push("stop");
         forwardOperations.add("stop");
      }
      for(int x = 0; x < manageRows.size(); x++)
      {
         scalarOperation(fraction, x, operations, forwardOperations);
      }
      if(addToStack)
      {
         operations.push("start");
         forwardOperations.add("stop");
      }
   }

   public void scalarOperation(Fraction multiple, int row, Stack<String> operations, ArrayList<String> forwardOperations)
   {
      if(row >= manageRows.size() || row < 0)
      {
         //System.out.println("Bad parameters");
         throw new IllegalArgumentException("Bad parameters");
      }

      if(addToStack)
      {
         operations.push(Fraction.reciprocal(multiple) + "r" + (row + 1));
         forwardOperations.add(multiple + "r" + (row + 1));
      }

      List<Fraction> list = manageRows.remove(row);

      for(int x = 0; x < list.size(); x++)
      {
         list.get(x).multiply(multiple);
      }

      manageRows.add(row, list);
   }

   public void makeWhole(int row, Stack<String> operations, ArrayList<String> forwardOperations)
   {
//      if(row >= manageRows.size() || row < 0)
//      {
//         //System.out.println("Bad parameters");
//         throw new IllegalArgumentException("Bad parameters");
//      }
//      List<Fraction> list = manageRows.get(row);
//
//      int largest = 1;
//      for(Fraction fraction: list)
//      {
//         largest = Math.max(largest, fraction.getDen());
//      }
//      Fraction toMultiply = new Fraction(largest, 1);
//
//      if(addToStack)
//      {
//         operations.push(Fraction.reciprocal(toMultiply) + "r" + (row + 1));
//         forwardOperations.add(toMultiply + "r" + (row + 1));
//      }
//
//      for(Fraction fraction: list)
//      {
//         fraction.multiply(toMultiply);
//      }
   }

   public static Matrix addMatrix(Matrix matrix1, Matrix matrix2)
   {
      Matrix newMatrix = new Matrix(matrix1.getColumns());

      List<Fraction> list;
      Fraction temp;

      for(int x = 0; x < matrix1.getRows(); x++)
      {
         list = new ArrayList<>();
         for(int y = 0; y < matrix1.getColumns(); y++)
         {
            temp = new Fraction(0, 1);
            temp.add(matrix1.manageRows.get(x).get(y));
            temp.add(matrix2.manageRows.get(x).get(y));
            list.add(temp);
         }
         newMatrix.addRow(list);
      }

      return newMatrix;
   }

   public void rowEchelon(Stack<String> operations, ArrayList<String> forwardOperations)
   {
      int y = 0;
      boolean keepGoing = true;

      if(addToStack)
      {
         operations.push("stop");
         forwardOperations.add("stop");
      }

      for(int x = 0; x < manageRows.size(); x++)
      {
         keepGoing = true;
         while(keepGoing)
         {
            if (findGoodSwap(x, y, operations, forwardOperations))
            {
               reduceToOne(x, y, operations, forwardOperations);
               keepGoing = false;
            }
            y++;
            if (y == manageRows.get(x).size() - 1)
            {
               break;
            }
         }
         if(y == manageRows.get(x).size() -1)
         {
            break;
         }
      }

      if(addToStack)
      {
         operations.push("start");
         forwardOperations.add("start");
      }
   }

   //public void undo()

   public int getColumns()
   {
      return this.columns;
   }

   public int getRows()
   {
      return this.manageRows.size();
   }

   public static Matrix multiply(Matrix matrix1, Matrix matrix2)
   {
      Matrix newMatrix = new Matrix(matrix2.getColumns());

      List<Fraction> list;
      List<Fraction> temp;
      Fraction toAdd, fracTemp;

      for(int x = 0; x < matrix1.getRows(); x++)
      {
         list = new ArrayList<>();
         temp = matrix1.manageRows.get(x);

         for(int y = 0; y < matrix2.getColumns(); y++)
         {
            toAdd = new Fraction(0, 1);
            for(int z = 0; z < temp.size(); z++)
            {
               fracTemp = new Fraction(0, 1);
               fracTemp.add(temp.get(z));
               fracTemp.multiply(matrix2.manageRows.get(z).get(y));
               toAdd.add(fracTemp);
            }
            list.add(toAdd);
         }
         newMatrix.addRow(list);
      }
      return newMatrix;
   }

   private void reduceToOne(int row, int col, Stack<String> operations, ArrayList<String> forwardOperations)
   {
      Fraction frac = manageRows.get(row).get(col);
      scalarOperation(new Fraction(frac.getDen(), frac.getNum()), row, operations, forwardOperations);
      Fraction temp, temp2;

      for(int x = 0; x < manageRows.size(); x++)
      {
         temp2 = manageRows.get(x).get(col);
         if(temp2 != frac)
         {
            temp = new Fraction(temp2.getNum(), temp2.getDen());
            if(!temp.isZero())
            {
               scalarOperation(new Fraction(temp.getNum(), temp.getDen()), row, operations, forwardOperations);
               addOperation(new Fraction(-1, 1), row, new Fraction(1, 1), x, operations, forwardOperations);
               scalarOperation(new Fraction(temp.getDen(), temp.getNum()), row, operations, forwardOperations);
            }
         }
      }
   }

   private boolean findGoodSwap(int row, int col, Stack<String> operations, ArrayList<String> forwardOperations)
   {
      if(manageRows.get(row).get(col).isZero())
      {
         for(int x = row; x < manageRows.size(); x++)
         {
            if(!manageRows.get(x).get(col).isZero())
            {
               swapOperation(row, x, operations, forwardOperations);
            }
         }
      }
      return !manageRows.get(row).get(col).isZero();
   }

   private static int getDev(int first, int second)
   {
      int dev = Math.min(Math.abs(first), Math.abs(second));
      dev = Math.max(1,dev);

      while(first%dev != 0 || second%dev != 0)
      {
         dev -= 1;
      }

      return dev;
   }

   @Override
   public Matrix clone()
   {
      try
      {
         super.clone();
      }
      catch(CloneNotSupportedException e)
      {
         System.out.println(e.getMessage());
         return null;
      }
      Matrix newMatrix = new Matrix(columns);
      Fraction tempFraction;
      List<Fraction> tempList;

      for(List<Fraction> fractions : manageRows)
      {
         tempList = new ArrayList<>();
         for(Fraction fraction : fractions)
         {
            tempFraction = new Fraction(fraction.getNum(), fraction.getDen());
            tempList.add(tempFraction);
         }
         newMatrix.addRow(tempList);
      }
      return newMatrix;
   }

   public void stopStack()
   {
      addToStack = false;
   }

   public void startStack()
   {
      addToStack = true;
   }

   public void forceForward()
   {
      forceForward = true;
   }

   public void releaseForward()
   {
      forceForward = false;
   }
}