package Input;

public enum RegexManagement
{
   Print("^print$"),
   Echelon("^echelon$"),
   Exit("^exit$"),
   ViewMatrices("^vm$"),
   Help("^help$"),
   Whole("^whole" + StaticStrings.row + "$"),
   AddMatrices("^" + StaticStrings.AdditiveScale + StaticStrings.matrix + "\\+" + StaticStrings.AdditiveScale + StaticStrings.matrix + "$"),
   SubtractMatrices("^" + StaticStrings.AdditiveScale + StaticStrings.matrix + "-" + StaticStrings.AdditiveScale + StaticStrings.matrix + "$"),
   MultiplyMatrices("^" + StaticStrings.AdditiveScale + StaticStrings.matrix + "x" + StaticStrings.AdditiveScale + StaticStrings.matrix + "$"),
   ScaleMatrix("^" + StaticStrings.AdditiveScale + StaticStrings.matrix + "$"),
   RemoveMatrices("^r" + StaticStrings.matrix + "$"),
   ChangeCurrent("^c" + StaticStrings.matrix + "$"),
   NewMatrix(StaticStrings.newAdditiveMatrix),
   SwapRows("^" + StaticStrings.row + "<>" + StaticStrings.row + "$"),
   AddRows("^" + StaticStrings.AdditiveScale + StaticStrings.row + "\\+" + StaticStrings.AdditiveScale + StaticStrings.row + "$"),
   SubtractRows("^" + StaticStrings.AdditiveScale + StaticStrings.row + "-" + StaticStrings.AdditiveScale + StaticStrings.row + "$"),
   ScaleRow("^" + StaticStrings.AdditiveScale + StaticStrings.row + "$"),
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