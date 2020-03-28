using DDHCenter.Core.Models;
using System;
using System.Collections.ObjectModel;

namespace DDHCenter.Core.ViewModels
{
    public class CategoryComboBoxControlViewModel : ViewModel
    {
        public ObservableCollection<Categoria> Categories { get; set; }

        public Categoria Categoria { get; set; }

        public CategoryComboBoxControlViewModel()
        {
            Categoria = null;
            Categories = null;
        }

        public CategoryComboBoxControlViewModel(ObservableCollection<Categoria> _categories)
        {
            Categoria = null;
            Categories = _categories;
        }
    }
}
