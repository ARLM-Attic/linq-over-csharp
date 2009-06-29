<#+
//
// Helper class for T4 code generator templates
//
public static class CodeGenHelper
{
  // --------------------------------------------------------------------------------------------
  /// <summary>
  /// Default working drive, can be overridden with the "LOCDrive" environment variable.
  /// </summary>
  // --------------------------------------------------------------------------------------------
  private const string WorkingDrive = @"C:\";

  // --------------------------------------------------------------------------------------------
  /// <summary>
  /// Default solution path, can be overridden with the "LOCPath" environment variable.
  /// </summary>
  // --------------------------------------------------------------------------------------------
  private const string WorkingPath = @"Work\LINQOverCSharp\CSharpFactoryVS2010";
    
  // --------------------------------------------------------------------------------------------
  /// <summary>
  /// Gets the current working folder.
  /// </summary>
  // --------------------------------------------------------------------------------------------
  private static string WorkingFolder
  {
    get
    {
      string locDrive = Environment.GetEnvironmentVariable("LOCDrive") ?? WorkingDrive;
      if (!locDrive.EndsWith(@"\")) 
      {
        locDrive += @"\"; 
      }
      string locPath = Environment.GetEnvironmentVariable("LOCPath") ?? WorkingPath;
      return Path.Combine(Path.Combine(locDrive, locPath), @"CSharpTreeBuilder\CSharpAst\CodeGenHelper");
    }
  }

  //
  // Returns all SyntaxNode type names (extracted from filenames found in SyntaxNode_filelist.txt)
  // filtered with typenames contained in SyntaxNode_ignore.txt
  // in a sorted list.
  //
  public static SortedList<string,string> GetTypeNameList()
  {
		SortedList<string,string> typeNameList = new SortedList<string,string>();
		List<string> ignoredTypeNames = new List<string>();

		using (StreamReader streamReader = new StreamReader(Path.Combine(WorkingFolder,@"SyntaxNode_ignore.txt")))
		{
			while (!streamReader.EndOfStream)
			{
				ignoredTypeNames.Add(streamReader.ReadLine().Trim());
			}
		}

		using (StreamReader streamReader = new StreamReader(Path.Combine(WorkingFolder,@"SyntaxNode_filelist.txt")))
		{
			while (!streamReader.EndOfStream)
			{
				string line = streamReader.ReadLine();
				int pathEndIndex = line.LastIndexOf('\\');
				if (pathEndIndex >= 0)
				{
					string typeName = line.Substring(pathEndIndex + 1, line.Length - pathEndIndex - 4);
					if (!ignoredTypeNames.Contains(typeName))
					{
						typeNameList.Add(typeName,null);
					}
				}
			}
		}

		return typeNameList;	
  }
}
#>
