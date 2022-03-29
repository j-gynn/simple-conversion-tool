using System;
using System.IO;
using System.Collections.Generic;

namespace COMP1003
{

    /*
     * WHAT IS NEEDED
     * 
     * function to read the file
     * 1 - read file
     * 2 - check how long the file is (FL) + create array of that length (array[FL][])
     * 3 - use a for loop using the aforementioned length to read through line-by-line
     * 4 - populate array with file contents
     * 
     * function to separate into comparable units
     * 1 - using for loop, populate second part of array with each thing as separated from first
     * 2 - remove whitespace if it exists
     * 
     * function to perform comparison
     * 1 - do a case check on array[i][0] (basic type check) - convert & store the number as it is in SI units (kg, ml etc) as baseUnit
     * 2 - set a variable to determine type of unit
     * 3 - case check on array[i][1] - if its type is not the same as the other, populate answer[i] with -1
     * 4 - if it is the same, multiply baseUnit by the multiplier for that unit's ratio
     * 5 - save in answer[i]
     * 
     * function to return all calculations
     * 1 - for loop of length FL
     * 2 - new array of of length FL
     * 3 - check if answer[i] = -1, and if it does, populate array[i] with "Cannot calculate - units are incompatible"
     * 3 - populate array[i] as follows: array[i][3] + array[i][1] + "s are " + answer[i] + array[i][2] + "s" (x [unit]s are y [unit2]s)
     * 
     * 
     */

    class Program
    {

        public class Converter
        {
            //file length
            private int FL = 0;

            //The class for a single input
            public class Operation
            {
                public string rawInput;
                public string unit1;
                public string unit2;
                public int value;
            }

            List<Operation> Conversions = new List<Operation>();

            public string[] answer;

            public void readInput(string url)
            {
                using (StreamReader reader = new StreamReader(url))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Conversions.Add(new Operation() { rawInput = line });
                    }
                }
            }
        }





        static void Main(string[] args)
        {
            Converter converter = new Converter();

            converter.readInput("convert.txt");
        }
    }
}
