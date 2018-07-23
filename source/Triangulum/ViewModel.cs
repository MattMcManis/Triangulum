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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triangulum
{
    public class ViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string prop)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(prop));
            }
        }


        public ViewModel()
        {
            // -------------------------
            // Defaults
            // -------------------------
            Center_IsChecked = true;

            Wrap_IsChecked = false;
            Wrap_Text = "NoWrap";

            Binary_IsChecked = true;
            Binary0_IsChecked = false;
            Binary1_IsChecked = true;

            ASCII_IsChecked = false;

            Inline_IsChecked = false;

            Rows_Text = "200";
        }


        // -------------------------
        // Display
        // -------------------------
        public string _Display_Text;
        public string Display_Text
        {
            get { return _Display_Text; }
            set
            {
                if (_Display_Text == value)
                {
                    return;
                }

                _Display_Text = value;
                OnPropertyChanged("Display_Text");
            }
        }
        // Font Size
        public double _Display_FontSize = 12;
        public double Display_FontSize
        {
            get { return _Display_FontSize; }
            set
            {
                if (_Display_FontSize == value)
                {
                    return;
                }

                _Display_FontSize = value;
                OnPropertyChanged("Display_FontSize");
            }
        }
        // Text Wrap
        public string _Wrap_Text;
        public string Wrap_Text
        {
            get { return _Wrap_Text; }
            set
            {
                if (_Wrap_Text == value)
                {
                    return;
                }

                _Wrap_Text = value;
                OnPropertyChanged("Wrap_Text");
            }
        }


        // -------------------------
        // Rows
        // -------------------------
        public string _Rows_Text;
        public string Rows_Text
        {
            get { return _Rows_Text; }
            set
            {
                if (_Rows_Text == value)
                {
                    return;
                }

                _Rows_Text = value;
                OnPropertyChanged("Rows_Text");
            }
        }


        // -------------------------
        // Center - Toggle
        // -------------------------
        private bool _Center_IsChecked;
        public bool Center_IsChecked
        {
            get { return _Center_IsChecked; }
            set
            {
                if (_Center_IsChecked == value) return;

                _Center_IsChecked = value;
            }
        }


        // -------------------------
        // Wrap - Toggle
        // -------------------------
        private bool _Wrap_IsChecked;
        public bool Wrap_IsChecked
        {
            get { return _Wrap_IsChecked; }
            set
            {
                if (_Wrap_IsChecked == value) return;

                _Wrap_IsChecked = value;
            }
        }


        // -------------------------
        // Inline - Toggle
        // -------------------------
        private bool _Inline_IsChecked;
        public bool Inline_IsChecked
        {
            get { return _Inline_IsChecked; }
            set
            {
                if (_Inline_IsChecked == value) return;

                _Inline_IsChecked = value;
            }
        }


        // -------------------------
        // ASCII - Toggle
        // -------------------------
        private bool _ASCII_IsChecked;
        public bool ASCII_IsChecked
        {
            get { return _ASCII_IsChecked; }
            set
            {
                if (_ASCII_IsChecked == value) return;

                _ASCII_IsChecked = value;
            }
        }

        // -------------------------
        // Binary - Toggle
        // -------------------------
        private bool _Binary_IsChecked;
        public bool Binary_IsChecked
        {
            get { return _Binary_IsChecked; }
            set
            {
                if (_Binary_IsChecked == value) return;

                _Binary_IsChecked = value;
            }
        }

        // -------------------------
        // Binary 0 - Toggle
        // -------------------------
        private bool _Binary0_IsChecked;
        public bool Binary0_IsChecked
        {
            get { return _Binary0_IsChecked; }
            set
            {
                if (_Binary0_IsChecked == value) return;

                _Binary0_IsChecked = value;
            }
        }


        // -------------------------
        // Binary 1 - Toggle
        // -------------------------
        private bool _Binary1_IsChecked;
        public bool Binary1_IsChecked
        {
            get { return _Binary1_IsChecked; }
            set
            {
                if (_Binary1_IsChecked == value) return;

                _Binary1_IsChecked = value;
            }
        }


        // -------------------------
        // Sum - Toggle
        // -------------------------
        private bool _Sum_IsChecked;
        public bool Sum_IsChecked
        {
            get { return _Sum_IsChecked; }
            set
            {
                if (_Sum_IsChecked == value) return;

                _Sum_IsChecked = value;
            }
        }


        // -------------------------
        // Decimal - Toggle
        // -------------------------
        // Checked
        private bool _Decimal_IsChecked;
        public bool Decimal_IsChecked
        {
            get { return _Decimal_IsChecked; }
            set
            {
                if (_Decimal_IsChecked == value) return;

                _Decimal_IsChecked = value;
            }
        }

        // Enabled
        private bool _Decimal_IsEnabled = true;
        public bool Decimal_IsEnabled
        {
            get { return _Decimal_IsEnabled; }
            set
            {
                if (_Decimal_IsEnabled == value)
                {
                    return;
                }

                _Decimal_IsEnabled = value;
                OnPropertyChanged("Decimal_IsEnabled");
            }
        }




    }
}
