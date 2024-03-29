﻿using System.Collections.Generic;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace JSDG.Platform.UserControls
{
    public class ModernShowDialog : DpiAwareWindow
    {
        /// <summary>
        /// Identifies the BackgroundContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BackgroundContentProperty = DependencyProperty.Register("BackgroundContent", typeof(object), typeof(ModernShowDialog));

        /// <summary>
        /// Identifies the Buttons dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(IEnumerable<Button>), typeof(ModernShowDialog));

        private ICommand closeCommand;

        private Button okButton;
        private Button cancelButton;
        private Button yesButton;
        private Button noButton;
        private Button closeButton;

        private MessageBoxResult messageBoxResult = MessageBoxResult.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernShowDialog"/> class.
        /// </summary>
        public ModernShowDialog()
        {
            this.DefaultStyleKey = typeof(ModernShowDialog);
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            this.closeCommand = new RelayCommand(o =>
            {
                var result = o as MessageBoxResult?;
                if (result.HasValue)
                {
                    this.messageBoxResult = result.Value;

                    // sets the Window.DialogResult as well
                    if (result.Value == MessageBoxResult.OK || result.Value == MessageBoxResult.Yes)
                    {
                        this.DialogResult = true;
                    }
                    else if (result.Value == MessageBoxResult.Cancel || result.Value == MessageBoxResult.No)
                    {
                        this.DialogResult = false;
                    }
                    else
                    {
                        this.DialogResult = null;
                    }
                }
                Close();
            });

            this.Buttons = new Button[] { this.CloseButton };

            // set the default owner to the app main window (if possible)
            if (Application.Current != null && Application.Current.MainWindow != this && Application.Current.MainWindow.IsLoaded)
            {
                this.Owner = Application.Current.MainWindow;
            }
        }

        private Button CreateCloseDialogButton(string content, bool isDefault, bool isCancel, MessageBoxResult result)
        {
            return new Button
            {
                Content = content,
                Command = this.CloseCommand,
                CommandParameter = result,
                IsDefault = isDefault,
                IsCancel = isCancel,
                MinHeight = 21,
                MinWidth = 65,
                Margin = new Thickness(4, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.FloralWhite)
            };
        }

        /// <summary>
        /// Gets the close window command.
        /// </summary>
        public ICommand CloseCommand
        {
            get { return this.closeCommand; }
        }

        /// <summary>
        /// Gets the Ok button.
        /// </summary>
        public Button OkButton
        {
            get
            {
                if (this.okButton == null)
                {
                    this.okButton = CreateCloseDialogButton("确定", true, false, MessageBoxResult.OK);
                }
                return this.okButton;
            }
        }

        /// <summary>
        /// Gets the Cancel button.
        /// </summary>
        public Button CancelButton
        {
            get
            {
                if (this.cancelButton == null)
                {
                    this.cancelButton = CreateCloseDialogButton("确定", false, true, MessageBoxResult.Cancel);
                }
                return this.cancelButton;
            }
        }

        /// <summary>
        /// Gets the Yes button.
        /// </summary>
        public Button YesButton
        {
            get
            {
                if (this.yesButton == null)
                {
                    this.yesButton = CreateCloseDialogButton("确定", true, false, MessageBoxResult.Yes);
                }
                return this.yesButton;
            }
        }

        /// <summary>
        /// Gets the No button.
        /// </summary>
        public Button NoButton
        {
            get
            {
                if (this.noButton == null)
                {
                    this.noButton = CreateCloseDialogButton("确定", false, true, MessageBoxResult.No);
                }
                return this.noButton;
            }
        }

        /// <summary>
        /// Gets the Close button.
        /// </summary>
        public Button CloseButton
        {
            get
            {
                if (this.closeButton == null)
                {
                    this.closeButton = CreateCloseDialogButton("确定", true, false, MessageBoxResult.None);
                }
                return this.closeButton;
            }
        }

        /// <summary>
        /// Gets or sets the background content of this window instance.
        /// </summary>
        public object BackgroundContent
        {
            get { return GetValue(BackgroundContentProperty); }
            set { SetValue(BackgroundContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the dialog buttons.
        /// </summary>
        public IEnumerable<Button> Buttons
        {
            get { return (IEnumerable<Button>)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }

        /// <summary>
        /// Gets the message box result.
        /// </summary>
        /// <value>
        /// The message box result.
        /// </value>
        public MessageBoxResult MessageBoxResult
        {
            get { return this.messageBoxResult; }
        }

        /// <summary>
        /// Displays a messagebox.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="title">The title.</param>
        /// <param name="button">The button.</param>
        /// <param name="owner">The window owning the messagebox. The messagebox will be located at the center of the owner.</param>
        /// <returns></returns>
        public static MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Window owner = null)
        {
            var dlg = new ModernShowDialog
            {
                Title = title,
                Content = new BBCodeBlock { BBCode = text, Margin = new Thickness(0, 0, 0, 8) },
                MinHeight = 0,
                MinWidth = 0,
                MaxHeight = 480,
                MaxWidth = 640,
            };
            if (owner != null)
            {
                dlg.Owner = owner;
            }

            dlg.Buttons = GetButtons(dlg, button);
            dlg.ShowDialog();
            return dlg.MessageBoxResult;
        }

        private static IEnumerable<Button> GetButtons(ModernShowDialog owner, MessageBoxButton button)
        {
            if (button == MessageBoxButton.OK)
            {
                yield return owner.OkButton;
            }
            else if (button == MessageBoxButton.OKCancel)
            {
                yield return owner.OkButton;
                yield return owner.CancelButton;
            }
            else if (button == MessageBoxButton.YesNo)
            {
                yield return owner.YesButton;
                yield return owner.NoButton;
            }
            else if (button == MessageBoxButton.YesNoCancel)
            {
                yield return owner.YesButton;
                yield return owner.NoButton;
                yield return owner.CancelButton;
            }
        }
    }
}
