namespace xslt
{
  class XSLT
  {
    static void AbortIfFileNotExisting(string fileName)
    {
      if (!System.IO.File.Exists(fileName))
      {
        System.Console.Error.WriteLine(
            "File \"{0}\" does not exist.");
        System.Environment.Exit(1);
      }
    }

    static void CreateParentDirectoriesOrAbort(string outFile)
    {
      try
      {
        System.IO.Directory.CreateDirectory(
            System.IO.Path.GetDirectoryName(
            System.IO.Path.GetFullPath(outFile)));
      }
      catch (System.Exception ex)
      {
        System.Console.Error.WriteLine(
            "Failed to create output directories for \"{0}\":",
            outFile);
        System.Console.Error.WriteLine(ex);
        System.Environment.Exit(2);
      }
    }

    static int Main(string[] args)
    {
      try
      {
        if (args.Length != 3)
        {
          System.Console.WriteLine(
              "Usage: xslt XMLFILE XSLFILE OUTFILE");
          System.Console.WriteLine();
          System.Console.WriteLine(
              "Transforms XMLFILE into OUTFILE"
          + " using the XML Stylesheet Transform specified in XSLFILE.");

          return 0;
        }

        string xmlFile = args[0];
        string xslFile = args[1];
        string outFile = args[2];

        // Check the input files exist
        AbortIfFileNotExisting(xmlFile);
        AbortIfFileNotExisting(xslFile);

        var transform = new System.Xml.Xsl.XslCompiledTransform();

        // Load and compile the XSL file
        try
        {
          transform.Load(xslFile);
        }
        catch (System.Exception ex)
        {
          System.Console.Error.WriteLine(
              "Error loading XSL file: {0}",
              ex);
          return 2;
        }

        // Ensure any parent directories of the output file are created
        CreateParentDirectoriesOrAbort(outFile);

        // Transform the XML file into the output file
        try
        {
          transform.Transform(xmlFile, outFile);
        }
        catch (System.Exception ex)
        {
          System.Console.Error.WriteLine(
              "Error transforming XML file: {0}",
              ex);
          return 3;
        }

        // All done
        System.Console.WriteLine(
            "{0} => {1}",
            System.IO.Path.GetFileName(xmlFile),
            System.IO.Path.GetFileName(outFile));
        return 0;
      }
      catch (System.Exception ex)
      {
        System.Console.WriteLine(
            "Error: {0}", ex);
        return 4;
      }
    }
  }
}
