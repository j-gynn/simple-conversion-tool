using System;
using System.IO;

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

            //whether the program should ask for further input
            public bool active = true;

            //The class for a single input
            public class Operation
            {
                public string rawInput;
                public Unit unit1;
                public Unit unit2;
                public double value;
                public double answer;
                public bool userInput;
            }

            //The array that stores all inputted values
            Operation[] Conversions;

            public class Unit
            {
                public string name;
                public string displayname;
                public int category;
                public double SIconvert;

            }

            readonly Unit[] Units = {
                new Unit() { name = "error", category = -1, SIconvert = 0 },

                new Unit() { name = "kilogram", category = 1, SIconvert = 1 },
                new Unit() { name = "gram", category = 1, SIconvert = 0.001 },
                new Unit() { name = "ton", category = 1, SIconvert = 1016.05 },
                new Unit() { name = "uston", displayname = "US ton", category = 1, SIconvert = 907.185 },
                new Unit() { name = "stone", category = 1, SIconvert = 6.35029 },
                new Unit() { name = "pound", category = 1, SIconvert = 0.453592 },
                new Unit() { name = "ounce", category = 1, SIconvert = 0.0283495 },

                new Unit() { name = "meter", category = 2, SIconvert = 1 },
                new Unit() { name = "kilometer", category = 2, SIconvert = 1000 },
                new Unit() { name = "centimeter", category = 2, SIconvert = 0.01 },
                new Unit() { name = "millimeter", category = 2, SIconvert = 0.001 },
                new Unit() { name = "mile", category = 2, SIconvert = 1609.34 },
                new Unit() { name = "yard", category = 2, SIconvert = 0.9144 },
                new Unit() { name = "foot", category = 2, SIconvert = 0.3048 },
                new Unit() { name = "inch", category = 2, SIconvert = 0.0254 },
                new Unit() { name = "nauticalmile", displayname = "nautical mile", category = 2, SIconvert = 1852 },
                new Unit() { name = "furlong", category = 2, SIconvert = 0.00497096 },
                new Unit() { name = "chain", category = 2, SIconvert = 20.1168 },
                new Unit() { name = "rod", category = 2, SIconvert = 5.0292 },
                new Unit() { name = "link", category = 2, SIconvert = 0.201168 },
                new Unit() { name = "hand", category = 2, SIconvert = 0.1016 },

                new Unit() { name = "liter", category = 3, SIconvert = 1 },
                new Unit() { name = "pint", category = 3, SIconvert = 0.568261 },
                new Unit() { name = "cup", category =3, SIconvert = 0.2841304749995838 },
                new Unit() { name = "quart", category = 3, SIconvert = 1.136521999998291399},
                new Unit() { name = "tablespoon", category = 3, SIconvert = 0.017758163587470073008},
                new Unit() { name = "teaspoon", category = 3, SIconvert = 0.0059193878628233569453}




        };


            public string answer;

            
            public string getInput()
            {
                return Console.ReadLine();
            }

            public void readInput(string url)
            {
                answer = "";
                string line;
                string block = "";
                string[] rawInput;
                bool userinput = false;
                try
                {
                    using (StreamReader reader = new StreamReader(url))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            block += line + "_";
                            FL++;
                        }
                    }
                    rawInput = block.Split('_');
                } 
                //if we get here, we may be dealing with a single input from the user
                //we will attempt to process it as such
                catch
                {
                    rawInput = new string[1];
                    rawInput[0] = url;
                    FL = 1;
                    userinput = true;
                }

                Conversions = new Operation[FL];

                for (int i = 0; i < FL; i++)
                {
                    Conversions[i] = new Operation();
                    Conversions[i].rawInput = rawInput[i];
                    if (userinput)
                    {
                        Conversions[i].userInput = true;
                    }
                }
            }

            public void separate()
            {
                string[] separated;
                int unit1pos = 0;
                int unit2pos = 1;
                int valuepos = 2;

                if (Conversions[0].userInput)
                {
                    valuepos = 0;
                    unit1pos = 1;
                    unit2pos = 2;
                }

                for (int i = 0; i < FL; i++)
                {
                    separated = Conversions[i].rawInput.Split(',');

                    if (separated.Length < 3)
                    {
                        Console.WriteLine("Error! Unable to parse input.");
                        Conversions[i].answer = -4;
                        break;
                    }

                    for (int j = 0; j < separated.Length; j++)
                    {
                        separated[j] = separated[j].Trim().ToLower();

                    }

                    //iterating through the known units to find the ones we need
                    for (int j = 0; j < Units.Length; j++)
                    {
                        if (separated[unit1pos] == Units[j].name)
                        {
                            Conversions[i].unit1 = Units[j];
                        }
                        if (separated[unit2pos] == Units[j].name)
                        {
                            Conversions[i].unit2 = Units[j];
                        }
                    }

                    //if a match for the units couldn't be found, set them to the error unit to allow normal procedure to continue
                    //this will be corrected for at the end
                    if (Conversions[i].unit1 == null) {
                        Conversions[i].unit1 = Units[0];
                    }
                    if (Conversions[i].unit2 == null)
                    {
                        Conversions[i].unit2 = Units[0];
                    }

                    Conversions[i].value = Convert.ToDouble(separated[valuepos]);
                }
            }

            public void convert()
            {
                for (int i = 0; i < FL; i++)
                {
                    //if there was a parsing issue in the previous stage
                    if (Conversions[i].answer == -4)
                    {
                        break;
                    }
                    //if the input value is less than zero
                    else if (Conversions[i].value <= 0)
                    {
                        Conversions[i].answer = -3;



                        //if this was submitted by a user, exit the program
                        if (Conversions[i].userInput)
                        {
                            answer += "Program exiting.\n";
                            active = false;
                        }
                        else
                        {
                            answer += "Cannot calculate: value is less than zero\n";
                        }

                    }
                    //if either of the units were unrecognised 
                    else if ( Conversions[i].unit1.name == "error" || Conversions[i].unit2.name == "error")
                    {
                        Conversions[i].answer = -2;

                        answer += "Cannot calculate: one or both units not recognised\n";
                        
                    }
                    //if the type of unit does not match
                    else if (Conversions[i].unit1.category != Conversions[i].unit2.category)
                    {
                        Conversions[i].answer = -1;

                        answer += "Cannot calculate: units are incompatible\n";
                    } 
                   else {
                        string unit1;
                        string unit2;
                        Conversions[i].answer = (Conversions[i].value * Conversions[i].unit1.SIconvert) / Conversions[i].unit2.SIconvert;

                        //for display purposes, if one of the unit names normally has a space (such as imperial ton) and/or capital letters (as in US ton), it can be replaced here
                        //I know there's no points awarded for this, it was just bugging me personally. don't get me started on the pluralisation stuff.
                        if (Conversions[i].unit1.displayname != null)
                        {
                            unit1 = Conversions[i].unit1.displayname;
                        } else
                        {
                            unit1 = Conversions[i].unit1.name;
                        }

                        if (Conversions[i].unit2.displayname != null)
                        {
                            unit2 = Conversions[i].unit2.displayname;
                        }
                        else
                        {
                            unit2 = Conversions[i].unit2.name;
                        }

                        answer += Conversions[i].value.ToString() + " " + unit1 + "s are " + Conversions[i].answer + " " + unit2 + "s\n";
                    }
                }
                Console.Write(answer);
            }
        }





        static void Main(string[] args)
        {
            Converter converter = new Converter();

            converter.readInput("convert.txt");
            converter.separate();
            converter.convert();

            while (converter.active)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Please enter your conversion:");
                string input = converter.getInput();
                converter.readInput(input);
                converter.separate();
                converter.convert();
            }
            Console.WriteLine("\nGoodbye!");
        }
    }
}
