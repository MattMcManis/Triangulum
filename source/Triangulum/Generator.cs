/* ----------------------------------------------------------------------
Triangulum
Copyright (C) 2018-2019 Matt McManis
http://github.com/MattMcManis/Triangulum
mattmcmanis@outlook.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see <http://www.gnu.org/licenses/>. 
---------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Numerics;

namespace Triangulum
{
    public class Generator
    {
        /// <summary>
        ///     Pascals Triangle - Method
        /// </summary>
        /// <remarks>
        ///     Credit: https://www.w3resource.com/csharp-exercises/for-loop/csharp-for-loop-exercise-33.php
        ///     Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License
        /// </remarks>

        public static void PascalsTriangle(ViewModel vm, int rows)
        {
            // Progress Info
            vm.Display_Text = "Generating...";

            List<string> triangle = new List<string>();

            // System.Numerics.BigInteger
            BigInteger row_no;
            BigInteger c = 1;
            int i, j;

            row_no = rows;
            for (i = 0; i < row_no; i++)
            {
                List<BigInteger> triangle_row = new List<BigInteger>();

                for (j = 0; j <= i; j++)
                {
                    if (j == 0 || i == 0)
                        c = 1;
                    else
                        c = c * (i - j + 1) / j;

                    // Convert to Binary
                    if (vm.Binary_IsChecked == true)
                    {
                        // Odd = 1
                        if (c % 2 != 0)
                        {
                            triangle_row.Add(1);
                        }
                        // Even = 0
                        else
                        {
                            triangle_row.Add(0);
                        }
                    }

                    // Integers
                    else
                    {
                        triangle_row.Add(c);
                    }

                }

                // -------------------------
                // Decimal
                // -------------------------
                if (vm.Decimal_IsChecked == true)
                {
                    string join = string.Join("", triangle_row);

                    // Convert Binary to Decimal
                    BigInteger sequence = BinaryToDec(join);

                    // Add to Triangle
                    triangle.Add(Convert.ToString(sequence));
                }

                // -------------------------
                // Sum
                // -------------------------
                if (vm.Sum_IsChecked == true)
                {
                    BigInteger sum = triangle_row.Aggregate(BigInteger.Add);

                    triangle.Add(string.Join(" ", sum));
                }

                // -------------------------
                // Individual
                // -------------------------
                if (vm.Decimal_IsChecked == false &&
                    vm.Sum_IsChecked == false)
                {
                    triangle.Add(string.Join(" ", triangle_row));
                }

            } // End Loop


            // -------------------------
            // Center
            // -------------------------
            if (vm.Center_IsChecked == true)
            {
                var maxLength = triangle.Last().Length;
                for (i = 0; i < rows; i++)
                {
                    triangle[i] = new string(' ', (maxLength - triangle[i].Length) / 2) + triangle[i];
                }
            }


            // -------------------------
            // Output
            // -------------------------
            string output = string.Join("\r\n", triangle);


            // -------------------------
            // Convert to Binary
            // -------------------------
            if (vm.Binary_IsChecked == true)
            {
                // Binary 0's
                if (vm.Binary0_IsChecked == true &&
                    vm.Binary1_IsChecked == false)
                {
                    output = Regex.Replace(output, "1", " ");
                }

                // Binary 1's
                if (vm.Binary1_IsChecked == true &&
                    vm.Binary0_IsChecked == false)
                {
                    output = Regex.Replace(output, "0", " ");
                }

                // Binary 1's & 0's
                // Do not Regex Replace
            }

            // -------------------------
            // Convert to ASCII
            // -------------------------
            if (vm.ASCII_IsChecked == true)
            {
                string data = string.Empty;
                data = Regex.Replace(output, " ", "");
                data = Regex.Replace(output, "\r\n", "");
                var stream = ConvertToBytes(data);
                output = Encoding.ASCII.GetString(stream);
            }


            // -------------------------
            // Number Distribution
            // -------------------------
            if (vm.NumberDistribution_IsChecked == true)
            {
                output = "Number Distribution" + "\r\n" +
                         "\r\n" +
                         vm.Rows_Text + " Rows" + "\r\n" +
                         "\r\n" +
                         "0:  " + output.Count(d => d == '0') + "\r\n" +
                         "1:  " + output.Count(d => d == '1') + "\r\n" +
                         "2:  " + output.Count(d => d == '2') + "\r\n" +
                         "3:  " + output.Count(d => d == '3') + "\r\n" +
                         "4:  " + output.Count(d => d == '4') + "\r\n" +
                         "5:  " + output.Count(d => d == '5') + "\r\n" +
                         "6:  " + output.Count(d => d == '6') + "\r\n" +
                         "7:  " + output.Count(d => d == '7') + "\r\n" +
                         "8:  " + output.Count(d => d == '8') + "\r\n" +
                         "9:  " + output.Count(d => d == '9');
            }

            // -------------------------
            // Inline
            // -------------------------
            if (vm.Inline_IsChecked == true)
            {
                output = Regex.Replace(output, " ", "");
                output = Regex.Replace(output, "\r\n", "");
            }

            // -------------------------
            // Display
            // -------------------------
            vm.Display_Text = output;
        }


        /// <summary>
        ///     Binary to Decimal
        /// </summary>
        /// <remarks>
        ///     https://stackoverflow.com/a/44711854/6806643
        /// </remarks>
        static BigInteger BinaryToDec(string input)
        {
            char[] array = input.ToCharArray();
            // Reverse since 16-8-4-2-1 not 1-2-4-8-16. 
            Array.Reverse(array);
            /*
             * [0] = 1
             * [1] = 2
             * [2] = 4
             * etc
             */
            BigInteger sum = 0;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == '1')
                {
                    // Method uses raising 2 to the power of the index. 
                    if (i == 0)
                    {
                        sum += 1;
                    }
                    else
                    {
                        sum += (BigInteger)Math.Pow(2, i);
                    }
                }

            }

            return sum;
        }


        /// <summary>
        ///     Binary to ASCII
        /// </summary>
        private static byte[] ConvertToBytes(string s)
        {
            byte[] result = new byte[(s.Length + 7) / 8];

            int i = 0;
            int j = 0;
            foreach (char c in s)
            {
                result[i] <<= 1;
                if (c == '1')
                    result[i] |= 1;
                j++;
                if (j == 8)
                {
                    i++;
                    j = 0;
                }
            }
            return result;
        }


    }
}
