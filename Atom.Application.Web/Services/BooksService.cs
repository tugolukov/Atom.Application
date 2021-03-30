using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Atom.Application.Web.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Atom.Application.Web.Services
{
    /// <summary>
    /// Сервис для работы с книгами
    /// </summary>
    public class BooksService
    {
        private readonly DatabaseContext _context;

        /// <summary/>
        public BooksService(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение книг по произвольному запросу
        /// </summary>
        /// <param name="byAuthorSearch">Поиск по автору</param>
        /// <param name="byCategorySearch">Поиск по разделу</param>
        /// <param name="isExist">Поиск по наличию</param>
        /// <returns>Список моделей униг</returns>
        public async Task<List<Book>> Search(string byAuthorSearch = "", string byCategorySearch = "", bool? isExist = null)
        {
            var query = _context.Books.AsQueryable();
            
            if (byAuthorSearch != "")
            {
                var byAuthor = byAuthorSearch.ToLower();
                
                query = query.Where(a => a.Author.ToLower().Contains(byAuthor));
            }
            
            if (byCategorySearch != "")
            {
                var byCategory = byCategorySearch.ToLower();
                
                query = query.Where(a => a.Category.ToLower().Contains(byCategory));
            }
            
            if (isExist.HasValue)
            {
                var isExistValue = isExist.Value;
                
                query = query.Where(a => a.IsExist == isExistValue);
            }
            
            return await query.ToListAsync();
        }
        
        /// <summary>
        /// Добавление книги
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task<Guid> Create(Book book)
        {
            book.Id = Guid.NewGuid();
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book.Id;
        }

        /// <summary>
        /// Редактирование книги
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        /// <param name="book">Модель книги</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Guid> Update(Guid id, Book book)
        {
            var existingBook = await _context.Books.SingleOrDefaultAsync(a => a.Id == id);
            if (existingBook == null)
            {
                throw new Exception("Книги с таким ключом не существует");
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Category = book.Category;
            existingBook.Publishing = book.Publishing;
            existingBook.IsExist = book.IsExist;
            existingBook.Rating = book.Rating;

            await _context.SaveChangesAsync();

            return existingBook.Id;
        }

        /// <summary>
        /// Удаление книги
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Delete(Guid id)
        {
            var book = await _context.Books.SingleOrDefaultAsync(a => a.Id == id);
            if (book == null)
            {
                throw new Exception("Книги с таким ключом не существует");
            }

            _context.Books.Remove(book);
        }

        /// <summary>
        /// Получение всей коллекции в формате JSON
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetJson()
        {
            var books = await _context.Books.ToListAsync();
            var json = JsonConvert.SerializeObject(books, Formatting.Indented);
            
            return json;
        }

        /// <summary>
        /// Сохранение данных из JSON
        /// </summary>
        /// <param name="json">Содержимое JSON</param>
        /// <returns></returns>
        public async Task SaveFromJson(string json)
        {
            var books = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
            books.ForEach(a => a.Id = Guid.NewGuid());

            await _context.AddRangeAsync(books);
            await _context.SaveChangesAsync();
        }
    }
}