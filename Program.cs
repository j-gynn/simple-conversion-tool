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
     * 2 - if the unit isn't recognised, set answer[i] to -2
     * 3 - set a variable to determine type of unit
     * 4 - case check on array[i][1] - if its type is not the same as the other, populate answer[i] with -1 (and if unrecognised, answer[i] = -2)
     * 5 - if it is the same, multiply baseUnit by the multiplier for that unit's ratio
     * 6 - save in answer[i]
     * 
     * function to return all calculations
     * 1 - for loop of length FL
     * 2 - new array of of length FL
     * 3 - check if answer[i] = -1, and if it does, populate array[i] with "Cannot calculate - units are incompatible"
     * 4 - check if answer[i] = -2, and if it does, populate array[i] with "Cannot calculate - one or both units not recognised"
     * 5 - populate array[i] as follows: array[i][3] + array[i][1] + "s are " + answer[i] + array[i][2] + "s" (x [unit]s are y [unit2]s)
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
                public Unit unit1;
                public Unit unit2;
                public double value;
                public double valueSI;
                public double answer;
            }

            List<Operation> Conversions = new List<Operation>();

            public class Unit
            {
                public string name;
                public string displayname;
                public int category;
                public double SIconvert;

            }

            List<Unit> Units = new List<Unit>();


            public double[] answer;

            public void initialise()
            {
                Units.Add(new Unit() { name = "error", category = -1, SIconvert = 0 }); 
                
                Units.Add(new Unit() { name = "kilogram", category = 1, SIconvert = 1 });
                Units.Add(new Unit() { name = "gram", category = 1, SIconvert = 0.001  });
                Units.Add(new Unit() { name = "imperialton", displayname = "imperial ton", category = 1, SIconvert = 1016.05 });
                Units.Add(new Unit() { name = "uston", displayname = "US ton", category = 1, SIconvert = 907.185 });
                Units.Add(new Unit() { name = "stone", category = 1, SIconvert = 6.35029 });
                Units.Add(new Unit() { name = "pound", category = 1, SIconvert = 0.453592 });
                Units.Add(new Unit() { name = "ounce", category = 1, SIconvert = 0.0283495 });

                Units.Add(new Unit() { name = "meter", category = 2, SIconvert = 1 });
                Units.Add(new Unit() { name = "kilometer", category = 2, SIconvert = 1000 });
                Units.Add(new Unit() { name = "centimeter", category = 2, SIconvert = 0.01 });
                Units.Add(new Unit() { name = "millimeter", category = 2, SIconvert = 0.001 });
                Units.Add(new Unit() { name = "mile", category = 2, SIconvert = 1609.34 });
                Units.Add(new Unit() { name = "yard", category = 2, SIconvert = 0.9144 });
                Units.Add(new Unit() { name = "foot", category = 2, SIconvert = 0.3048 });
                Units.Add(new Unit() { name = "inch", category = 2, SIconvert = 0.0254 });
                Units.Add(new Unit() { name = "nauticalmile", displayname = "nautical mile", category = 2, SIconvert = 1852 });
                Units.Add(new Unit() { name = "furlong", category = 2, SIconvert = 0.00497096 });
                Units.Add(new Unit() { name = "chain", category = 2, SIconvert = 20.1168 });
                Units.Add(new Unit() { name = "rod", category = 2, SIconvert = 5.0292 });
                Units.Add(new Unit() { name = "link", category = 2, SIconvert = 0.201168 });
                Units.Add(new Unit() { name = "hand", category = 2, SIconvert = 0.1016 });


            }

            public string getInput()
            {
                return Console.ReadLine();
            }

            public void readInput(string url)
            {
                using (StreamReader reader = new StreamReader(url))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Conversions.Add(new Operation() { rawInput = line });
                    }
                    FL = Conversions.Count;
                }
            }

            public void separate()
            {
                string[] separated;
                for (int i = 0; i < FL; i++)
                {
                    separated = Conversions[i].rawInput.Split(',');

                    for (int j = 0; j > 3; j++)
                    {
                        separated[i] = separated[i].Trim().ToLower();

                    }

                    try
                    {
                        Conversions[i].unit1 = Units.Find(x => x.name.Equals(separated[0]));
                    } catch
                    {
                        Conversions[i].unit1 = Units[0];
                    }

                    try
                    {
                        Conversions[i].unit2 = Units.Find(x => x.name.Equals(separated[1]));
                    } catch
                    {
                        Conversions[i].unit2 = Units[0];
                    }



                    Conversions[i].value = Convert.ToDouble(separated[2]);
                }
            }

            public void convert()
            {
                for (int i = 0; i < FL; i++)
                {
                    //if either of the units were unrecognised
                    if ( Conversions[i].unit1.name == "error" || Conversions[i].unit1.name == "error")
                    {
                        answer[i] = -2;
                        break;
                    }

                    //if the type of unit does not match
                    if (Conversions[i].unit1.category != Conversions[i].unit2.category)
                    {
                        answer[i] = -1;
                        break;
                    }

                    answer[i] = (Conversions[i].value * Conversions[i].unit1.SIconvert) / Conversions[i].unit2.SIconvert;

                }
            }
        }





        static void Main(string[] args)
        {
            Converter converter = new Converter();
            converter.initialise();

            converter.readInput("convert.txt");
            converter.separate();


            string input = converter.getInput();
            converter.readInput(input);
            converter.separate();
            converter.convert();
        }
    }
}
