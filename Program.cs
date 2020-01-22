using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ProcessInputFile
{
    class Program
    {
        static void Main(string[] args)
        {
            bool repeatExercise = false;

            do {
                int counter = 0;
                int expectedFieldsPerRecord = 0;
                int actualRecordFieldCount = 0;
                bool validInputFile = false;
                string goAgain = null;
                string fullFileName = null;
                string inputFileName = null;
                string delimitationType = null;
                string correctlyFormattedRecordsFile = null;
                string incorrectlyFormattedRecordsFile = null;
                List<string> inputRecords = new List<string>();
                List<string> correctlyFormattedRecords = new List<string>();
                List<string> incorrectlyFormattedRecords = new List<string>();
                
                Console.WriteLine("\nWhere is the File Located?");
                string filePath = Console.ReadLine();
                
                if (!Directory.Exists(filePath)) {
                    Console.WriteLine("\n'" + filePath + "' is not a valid directory");
                    repeatExercise = true;
                    continue;
                }
                
                Console.WriteLine("\nThank you. This is also where the output files will be stored.");

                do {
                    try {
                        Console.WriteLine("\nWhat is the name of the input file?");
                        inputFileName = Console.ReadLine();
                        fullFileName = filePath + "\\" + inputFileName;
                        inputRecords = File.ReadAllLines(fullFileName).ToList();
                        validInputFile = true;
                    } catch (Exception e) {
                        Console.WriteLine(e);
                        Console.WriteLine("\nInvalid filename: '" + fullFileName + "' does not exist.");
                    }
                } while(!validInputFile);

                correctlyFormattedRecordsFile = filePath + "\\CorrectlyFormattedRecords.txt";
                incorrectlyFormattedRecordsFile = filePath + "\\IncorrectlyFormattedRecords.txt";
                File.Delete(correctlyFormattedRecordsFile);
                File.Delete(incorrectlyFormattedRecordsFile);

                string splitBy = null;

                do {
                    Console.WriteLine("\nIs the file format CSV (comma-separated values) or TSV (tab-separated-values)?");
                    
                    delimitationType = Console.ReadLine();
                    
                    switch (delimitationType.ToUpper()) {
                        case "CSV": 
                            splitBy = ",";
                            break;
                        case "TSV": 
                            splitBy = "\t";
                            break;
                        default: 
                            Console.WriteLine("\nInvalid Entry: '" + delimitationType + "'. Enter CSV or TSV");
                            break;
                    }
                } while(splitBy == null);
                
                while (expectedFieldsPerRecord <= 0) {
                    Console.WriteLine("\nHow many fields should each record contain?");
                    try {
                        expectedFieldsPerRecord = Int32.Parse(Console.ReadLine());
                        if (expectedFieldsPerRecord <= 0) {
                            Console.WriteLine("\nInvalid Entry. Must be a positive Integer.");
                        }
                    } catch (Exception e) {
                        Console.WriteLine(e);
                        Console.WriteLine("\nInvalid Entry. Must be a positive Integer.");
                    }
                }
                
                foreach (string record in inputRecords) 
                {
                    if (counter > 0) {
                        actualRecordFieldCount = record.Split(splitBy).Length;
                        if (actualRecordFieldCount == expectedFieldsPerRecord) {
                            correctlyFormattedRecords.Add(record);
                        } else {
                            incorrectlyFormattedRecords.Add(record);
                        }
                    }
                    counter++;
                }

                if (correctlyFormattedRecords.Count > 0) {
                    File.WriteAllLines(correctlyFormattedRecordsFile, correctlyFormattedRecords);                    
                }

                if (incorrectlyFormattedRecords.Count > 0) {
                    File.WriteAllLines(incorrectlyFormattedRecordsFile, incorrectlyFormattedRecords);                    
                }

                do {
                    Console.WriteLine("\nGo again? (Y/N)");
                    goAgain = Console.ReadLine();
                    if (String.Equals(goAgain, "Y", StringComparison.OrdinalIgnoreCase)) {
                        repeatExercise = true;
                    }
                } while(!String.Equals(goAgain, "Y", StringComparison.OrdinalIgnoreCase) && 
                    !String.Equals(goAgain, "N", StringComparison.OrdinalIgnoreCase));

            } while(repeatExercise);
        }
    }
}
