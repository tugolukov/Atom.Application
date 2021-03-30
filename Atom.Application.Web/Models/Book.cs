using System;
using System.ComponentModel.DataAnnotations;

namespace Atom.Application.Web.Models
{
    /// <summary>
    /// Книга
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Идентификатор книги
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование книги
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public string Author { get; set; }
        
        /// <summary>
        /// Раздел
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Издательство
        /// </summary>
        public string Publishing { get; set; }

        /// <summary>
        /// Наличие
        /// </summary>
        public bool IsExist { get; set; } = true;

        [Range(1, 10)]
        public int Rating { get; set; } = 5;

        public Book()
        {
            Id = Guid.NewGuid();
        }
    }
}