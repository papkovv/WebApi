using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class Form
{
    public int ContactId { get; set; }
    
    [Required(ErrorMessage = "Укажите Ваше имя")]
    [StringLength(20)]
    public string ContactName { get; set; }
    
    [Required(ErrorMessage = "Укажите Ваш email")]
    [StringLength(50)]
    [EmailAddress(ErrorMessage = "Email является некорректным")]
    public string ContactEmail { get; set; }
    
    [Required(ErrorMessage = "Укажите Ваш номер телефона")]
    [StringLength(10)]
    [Phone(ErrorMessage = "Телефон явлется некорректным")]
    public string ContactPhone { get; set; }
    
    public int MessageId { get; set; }
    
    [Required(ErrorMessage = "Напишите Ваше сообщение")]
    [StringLength(500)]
    public string MessageObject { get; set; }
    
    [Required(ErrorMessage = "Выберите тему сообщения")]
    public int ThemeId { get; set; }
}