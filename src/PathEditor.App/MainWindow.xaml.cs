﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PathEditor.App.Dialog;
using PathEditor.Core;
using PathEditor.Core.Backup;
using PathEditor.Core.EnvironmentVariablePath;
using PathEditor.Core.Paper;

namespace PathEditor.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IFormattedEnvironmentVariablePath
    {
        private FormattedProcessEnvironmentVariablePath _formattedProcess;

        public MainWindow()
        {
            InitializeComponent();
            _formattedProcess = new FormattedProcessEnvironmentVariablePath(this);
            DataContext = new MainWindowViewModel(
                new EnvironmentVariablePathSystem(),
                new BackupProcess(
                    new BackupFile(new AskFile()),
                    new QuestionToBackupDialog()
                ),
                new UserSystem()
            );
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var paper = new WihoutFakeBreakLine(new AutoBreakLine(new TextOf(((TextBox)sender).Text, ((TextBox)sender).SelectionStart)));
            ((TextBox) sender).Text = paper.Text;
            ((TextBox) sender).SelectionStart = paper.Cursor;
        }

        private void TextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                var paper = new WithoutCurrentLine(new TextOf(((TextBox)sender).Text, ((TextBox)sender).SelectionStart));
                ((TextBox)sender).Text = paper.Text;
                ((TextBox)sender).SelectionStart = paper.Cursor;
                e.Handled = true;
            }
        }

        string IFormattedEnvironmentVariablePath.System => SystemPathTextBox.Text;
        string IFormattedEnvironmentVariablePath.User => UserPathTextBox.Text;

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is TabControl tabControl && tabControl.SelectedItem == ProcessTab)
                RefreshProcessTexBox();
        }

        private void RefreshProcessTexBox()
        {
            ProcessPathTextBox.Text = _formattedProcess.Value;
        }
    }
}