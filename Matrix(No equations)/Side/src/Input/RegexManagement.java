package Input;

public enum RegexManagement
{
   Print("^print$"),
   Echelon("^echelon$"),
   Exit("^exit$"),
   ViewMatrices("^vm$"),
   Help("^help$"),
   Whole("^whole" + StaticStrings.row + "$"),
   AddMatrices("^" + StaticStrings.scale + StaticStrings.matrix + "\\+" + StaticStrings.scale + StaticStrings.matrix + "$"),
   SubtractMatrices("^" + StaticStrings.scale + StaticStrings.matrix + "-" + StaticStrings.scale + StaticStrings.matrix + "$"),
   MultiplyMatrices("^" + StaticStrings.scale + StaticStrings.matrix + "x" + StaticStrings.scale + StaticStrings.matrix + "$"),
   ScaleMatrix("^" + StaticStrings.scale + StaticStrings.matrix + "$"),
   RemoveMatrices("^r" + StaticStrings.matrix + "$"),
   ChangeCurrent("^c" + StaticStrings.matrix + "$"),
   NewMatrix(StaticStrings.newMatrix),
   SwapRows("^" + StaticStrings.row + "<>" + StaticStrings.row + "$"),
   AddRows("^" + StaticStrings.scale + StaticStrings.row + "\\+" + StaticStrings.scale + StaticStrings.row + "$"),
   SubtractRows("^" + StaticStrings.scale + StaticStrings.row + "-" + StaticStrings.scale + StaticStrings.row + "$"),
   ScaleRow("^" + StaticStrings.scale + StaticStrings.row + "$"),
   PrintAllOperations("^printoperations$"),
   Undo("^undo$"),
   Nothing(".*");

   private String regex;
   
   private RegexManagement(String regex)
   {
      this.regex = regex;
   }
   
   public String getRegex()
   {
      return this.regex;
   }
}