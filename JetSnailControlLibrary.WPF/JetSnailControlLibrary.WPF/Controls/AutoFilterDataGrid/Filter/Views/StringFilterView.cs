using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace JetSnailControlLibrary.WPF
{
    [TemplatePart(Name = PartInput, Type = typeof(TextBox))]
    public class StringFilterView : BaseFilterView<StringFilterViewModel>
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringFilterView" /> class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="subString"></param>
        public StringFilterView(PropertyInfo fieldInfo, string subString)
        {
            // Add DataContext
            ViewModel = new StringFilterViewModel(fieldInfo, subString);
            DataContext = ViewModel;
        }

        #endregion

        #region Method

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes (such as a
        ///     rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Get template parts
            mTextBox = GetTemplateChild(PartInput) as TextBox;
            // Binding
            if (mTextBox != null) mTextBox.Text = ((StringFilterViewModel) DataContext).Substring;
        }

        #endregion

        #region Private Properties

        internal const string PartInput = "PART_Input";

        private TextBox mTextBox;

        #endregion
    }
}