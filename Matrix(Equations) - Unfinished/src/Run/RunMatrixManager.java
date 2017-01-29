package Run;

import Matrix.MatrixManager;

import java.util.Scanner;

public class RunMatrixManager
{
   public static void main(String[] args)
   {
      MatrixManager matrixManager = new MatrixManager();
      String command;
      Scanner kb = new Scanner(System.in);
      boolean keepGoing = true;
      matrixManager.command("help");
      do
      {
         System.out.print(">: ");
         command = kb.nextLine();
         keepGoing = matrixManager.command(command);
      }while(keepGoing);
   }
}