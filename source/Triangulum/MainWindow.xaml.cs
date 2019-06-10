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
using System.Numerics;
using System.Linq;
using System.Windows.Documents;

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
        ///     RichTextBox
        /// </summary>
        public static Paragraph p = new Paragraph();


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
http://github.com/MattMcManis/Triangulum
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
        ///     Font Size - Slider
        /// </summary>
        // Double Click
        private void slFontSize_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Return to Default Size
            slFontSize.Value = 12;
        }
        // Mouse Up
        private void slFontSize_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Change Font Size
            vm.Display_FontSize = slFontSize.Value;
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
        ///     Center - CheckBox
        /// </summary>
        private void cbxCenter_Checked(object sender, RoutedEventArgs e)
        {
            // Uncheck Inline
            vm.Inline_IsChecked = false;

            // Uncheck ASCII
            vm.ASCII_IsChecked = false;

            // Uncheck Decimal
            vm.Decimal_IsChecked = false;

            // Uncheck Sum
            vm.Sum_IsChecked = false;
        }


        /// <summary>
        ///     Inline - CheckBox
        /// </summary>
        private void cbxInline_Checked(object sender, RoutedEventArgs e)
        {
            // Uncheck Center
            vm.Center_IsChecked = false;
        }


        /// <summary>
        ///     ASCII - CheckBox
        /// </summary>
        private void cbxASCII_Checked(object sender, RoutedEventArgs e)
        {
            // Uncheck Center
            vm.Center_IsChecked = false;

            // Uncheck Binary
            vm.Binary1_IsChecked = true;
            vm.Binary0_IsChecked = true;

            // Uncheck Decimal
            vm.Decimal_IsChecked = false;

            // Uncheck Sum
            vm.Sum_IsChecked = false;

            // Uncheck Number Distribution
            vm.NumberDistribution_IsChecked = false;
        }

        /// <summary>
        ///     Decimal - CheckBox
        /// </summary>
        private void cbxDecimal_Checked(object sender, RoutedEventArgs e)
        {
            // Uncheck Center
            vm.Center_IsChecked = false;

            // Check Binary 0 & 1
            vm.Binary0_IsChecked = true;
            vm.Binary1_IsChecked = true;

            // Uncheck ASCII
            vm.ASCII_IsChecked = false;

            // Uncheck Sum
            vm.Sum_IsChecked = false;
        }


        /// <summary>
        ///     Binary - CheckBox
        /// </summary>
        // Checked
        private void cbxBinary_Checked(object sender, RoutedEventArgs e)
        {
            // Check Binary 0 & 1
            vm.Binary0_IsChecked = true;
            vm.Binary1_IsChecked = true;


            // Enable Decimal
            vm.Decimal_IsEnabled = true;

        }

        // Unchecked
        private void cbxBinary_Unchecked(object sender, RoutedEventArgs e)
        {
            // Uncheck Binary 0 & 1
            vm.Binary0_IsChecked = false;
            vm.Binary1_IsChecked = false;

            // Uncheck Decimal
            vm.Decimal_IsChecked = false;

            // Disable Decimal
            vm.Decimal_IsEnabled = false;
        }

        // Binary 0 - CheckBox
        private void cbx0_Checked(object sender, RoutedEventArgs e)
        {
            // Check Binary if Unchecked
            vm.Binary_IsChecked = true;
        }

        // Binary 1 - CheckBox
        private void cbx1_Checked(object sender, RoutedEventArgs e)
        {
            // Check Binary if Unchecked
            vm.Binary_IsChecked = true;
        }


        /// <summary>
        ///     Sum - CheckBox
        /// </summary>
        // Checked
        private void cbxSum_Checked(object sender, RoutedEventArgs e)
        {
            // Uncheck Center
            vm.Center_IsChecked = false;

            // Uncheck Decimal
            vm.Decimal_IsChecked = false;

            // Uncheck ASCII
            vm.ASCII_IsChecked = false;
        }
        // Unchecked
        private void cbxSum_Unchecked(object sender, RoutedEventArgs e)
        {

        }



        /// <summary>
        ///     Number Distribution - CheckBox
        /// </summary>
        // Checked
        private void cbxNumberDistribution_Checked(object sender, RoutedEventArgs e)
        {
            // Uncheck Center
            vm.Center_IsChecked = false;

            // Uncheck Inline
            vm.Inline_IsChecked = false;

            // Uncheck ASCII
            vm.ASCII_IsChecked = false;
        }
        // Unchecked
        private void cbxNumberDistribution_Unchecked(object sender, RoutedEventArgs e)
        {

        }



        /// <summary>
        ///     Rows - TextBox
        /// </summary>
        private void tbxRows_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Only Allow Numbers or Backspace
            if ((!(e.Key >= Key.D0 && e.Key <= Key.D9) && e.Key != Key.Back) ||
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
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
                Generator.PascalsTriangle(vm, rows);
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
            // Rows Over 1000 Warning
            //
            if (int.Parse(vm.Rows_Text) >= 1000)
            {
                // Yes/No Dialog Confirmation
                //
                MessageBoxResult result = MessageBox.Show(
                                                        "Entering a high number of rows requires a lot of processing power and system memory, and could risk crashing your computer.\n\nContinue?",
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

            // Rows Under 1000
            //
            else
            {
                Go();
            }
        }


    }
}
