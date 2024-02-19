using System.ComponentModel.DataAnnotations;

namespace Blogg.ViewModels.Categories;

public class EditorCategoryViewModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Slug { get; set; }
}