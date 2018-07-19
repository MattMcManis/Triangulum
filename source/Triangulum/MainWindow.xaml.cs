/* ----------------------------------------------------------------------
Triangulum
Copyright (C) 2018 Matt McManis
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

using Triangulum.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace Triangulum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        ///     View Model
        /// </summary>
        public ViewModel vm = new ViewModel();


        /// <summary>
        ///     Main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // View Model
            DataContext = vm;


            // --------------------------------------------------
            // Load Saved Settings
            // --------------------------------------------------
            // Window Position
            // First time use
            if (Convert.ToDouble(Settings.Default["Left"]) == 0 
                && Convert.ToDouble(Settings.Default["Top"]) == 0)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            // Load Saved
            else
            {
                this.Top = Settings.Default.Top;
                this.Left = Settings.Default.Left;
                this.Height = Settings.Default.Height;
                this.Width = Settings.Default.Width;

                if (Settings.Default.Maximized)
                {
                    WindowState = WindowState.Maximized;
                }
            }
        }


        /// <summary>
        ///     Close / Exit (Method)
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            // Force Exit All Executables
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        // Save Window Position
        void Window_Closing(object sender, CancelEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Settings.Default.Top = RestoreBounds.Top;
                Settings.Default.Left = RestoreBounds.Left;
                Settings.Default.Height = RestoreBounds.Height;
                Settings.Default.Width = RestoreBounds.Width;
                Settings.Default.Maximized = true;
            }
            else
            {
                Settings.Default.Top = this.Top;
                Settings.Default.Left = this.Left;
                Settings.Default.Height = this.Height;
                Settings.Default.Width = this.Width;
                Settings.Default.Maximized = false;
            }

            Settings.Default.Save();

            // Exit
            e.Cancel = true;
            Environment.Exit(0);
        }


        /// <summary>
        ///     Info - Button
        /// </summary>
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(@"
Triangulum
Pascal's Triangle Generator
© 2018 Matt McManis
mattmcmanis@outlook.com
GPL-3.0

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program. If not, see https://www.gnu.org/licenses/.

Pascal's Triangle
By w3resource.com
CC BY-NC-SA 3.0

Roboto Mono Font
© Christian Robertson
Apache License 2.0");
        }


        /// <summary>
        ///     Clear - Button
        /// </summary>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            vm.Display_Text = string.Empty;
        }


        /// <summary>
        ///     Save - Button
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Save File'
            Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();

            //saveFile.InitialDirectory = inputDir;
            saveFile.RestoreDirectory = true;
            saveFile.Filter = "Text file (*.txt)|*.txt";
            saveFile.DefaultExt = ".txt";
            saveFile.FileName = "output";

            // Show save file dialog box
            Nullable<bool> result = saveFile.ShowDialog();

            // Process dialog box
            if (result == true)
            {
                // Save document
                File.WriteAllText(saveFile.FileName, vm.Display_Text, Encoding.Unicode);
            }
        }


        /// <summary>
        ///     Pascals Triangle - Method
        /// </summary>
        // Credit: https://www.w3resource.com/csharp-exercises/for-loop/csharp-for-loop-exercise-33.php
        // Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License
        public void PascalsTriangle(ViewModel vm, int row)
        {
            // Progress Info
            vm.Display_Text = "Generating...";

            List<string> triangle = new List<string>();

            System.Numerics.BigInteger no_row, c = 1, blk, i, j;

            string spacer = string.Empty;
            if (vm.Center_IsChecked == true)
            {
                spacer = " ";
            }

            no_row = row;
            for (i = 0; i < no_row; i++)
            {
                for (blk = 1; blk <= no_row - i; blk++)
                    triangle.Add(spacer);

                for (j = 0; j <= i; j++)
                {
                    if (j == 0 || i == 0)
                        c = 1;
                    else
                        c = c * (i - j + 1) / j;

                    // Convert to Binary
                    if (vm.Binary_IsChecked == true)
                        if (c % 2 != 0)
                            if (vm.Binary1_IsChecked == true)
                                triangle.Add("1 ");
                            else
                                triangle.Add("  ");
                        else
                            if (vm.Binary0_IsChecked == true)
                                triangle.Add("0 ");
                            else
                                triangle.Add("  ");

                    // Integers
                    else
                        triangle.Add(string.Format("{0} ", c));
                }

                // Linebreak
                triangle.Add("\n");
            }

            // Output
            string output = string.Join(string.Empty, triangle);

            // Convert to ASCII
            if (vm.ASCII_IsChecked == true)
            {
                string data = string.Empty;
                data = Regex.Replace(output, " ", "");
                data = Regex.Replace(output, "\n", "");
                var stream = convertToBytes(data);
                output = Encoding.ASCII.GetString(stream);
            }

            // Inline
            if (vm.Inline_IsChecked == true)
            {
                output = Regex.Replace(output, " ", "");
                output = Regex.Replace(output, "\n", "");
            }

            // Display
            vm.Display_Text = output;
        }


        /// <summary>
        ///     Binary to ASCII
        /// </summary>
        private byte[] convertToBytes(string s)
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


        /// <summary>
        ///     Font Size - Slider
        /// </summary>
        private void slFontSize_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            slFontSize.Value = 9;
        }


        /// <summary>
        ///     Display Text Wrap - CheckBox
        /// </summary>
        private void cbxWrap_Checked(object sender, RoutedEventArgs e)
        {
            vm.Wrap_Text = "Wrap";
        }

        private void cbxWrap_Unchecked(object sender, RoutedEventArgs e)
        {
            vm.Wrap_Text = "NoWrap";
        }


        /// <summary>
        ///     ASCII - CheckBox
        /// </summary>
        private void cbxASCII_Checked(object sender, RoutedEventArgs e)
        {
            // Uncheck Binary
            cbxBinary.IsChecked = true;
            cbx0.IsChecked = true;
            cbx1.IsChecked = true;

            vm.Binary_IsChecked = true;
            vm.Binary1_IsChecked = true;
            vm.Binary0_IsChecked = true;
        }

        /// <summary>
        ///     Binary - CheckBox
        /// </summary>
        // Checked
        private void cbxBinary_Checked(object sender, RoutedEventArgs e)
        {
            // Check Binary 0 & 1
            //cbx0.IsChecked = true;
            cbx1.IsChecked = true;

            vm.Binary_IsChecked = true;
            //vm.Binary0_IsChecked = true;
            vm.Binary1_IsChecked = true;
            
        }

        // Unchecked
        private void cbxBinary_Unchecked(object sender, RoutedEventArgs e)
        {
            // Uncheck Binary 0 & 1
            cbx0.IsChecked = false;
            cbx1.IsChecked = false;

            vm.Binary_IsChecked = false;
            vm.Binary0_IsChecked = false;
            vm.Binary1_IsChecked = false;
        }

        // Binary 0 - CheckBox
        private void cbx0_Checked(object sender, RoutedEventArgs e)
        {
            // Check Binary if Unchecked
            if (vm.Binary_IsChecked == false)
            {
                cbxBinary.IsChecked = true;
                vm.Binary_IsChecked = true;
            }
        }

        // Binary 1 - CheckBox
        private void cbx1_Checked(object sender, RoutedEventArgs e)
        {
            // Check Binary if Unchecked
            if (vm.Binary_IsChecked == false)
            {
                cbxBinary.IsChecked = true;
                vm.Binary_IsChecked = true;
            }
        }


        /// <summary>
        ///     Rows - TextBox
        /// </summary>
        private void tbxRows_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Only Allow Numbers or Backspace
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9) && e.Key != Key.Back ||
                e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = true;
            }
        }


        /// <summary>
        ///     Go - Method
        /// </summary>
        public void Go()
        {
            // Rows
            //
            int rows = 0;
            if (!string.IsNullOrWhiteSpace(tbxRows.Text))
            {
                rows = int.Parse(vm.Rows_Text);
            }

            // Start New Thread
            //
            Thread worker = new Thread(() =>
            {
                PascalsTriangle(vm, rows);
            });
            worker.IsBackground = true;

            // Start Download Thread
            //
            worker.Start();
        }


        /// <summary>
        ///     Go - Button
        /// </summary>
        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            // Rows Over 2500 Warning
            //
            if (int.Parse(vm.Rows_Text) >= 2500)
            {
                // Yes/No Dialog Confirmation
                //
                MessageBoxResult result = MessageBox.Show(
                                                        "Entering a high number of rows uses a lot of processing power and system memory and could risk crashing your computer.\n\nContinue?",
                                                        "Arithmetic Overflow Warning",
                                                        MessageBoxButton.YesNo,
                                                        MessageBoxImage.Warning
                                                        );
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        // Run
                        Go();
                        break;

                    case MessageBoxResult.No:
                        // Cancel
                        break;
                }
            }

            // Rows Under 2500
            //
            else
            {
                Go();
            }
        }


    }
}
