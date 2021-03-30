using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Atom.Application.Web.Models;
using Atom.Application.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Atom.Application.Web.Controllers
{
    /// <summary>
    /// API для работы с картотекой
    /// </summary>
    [Route("Main")]
    public class BooksController
    {
        private readonly BooksService _service;

        public BooksController(BooksService service) => _service = service;

        /// <summary>
        /// Вывод книг по произвольному запросу
        /// </summary>
        /// <param name="byAuthor">по автору</param>
        /// <param name="byCategory">по разделу</param>
        /// <param name="isExist">по наличию</param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<List<Book>> Search(
            [FromQuery] string byAuthor = "",
            [FromQuery] string byCategory = "",
            bool? isExist = null) => await _service.Search(byAuthor,
                                                           byCategory,
                                                           isExist);

        /// <summary>
        /// Добавление книги
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> Create([FromBody] Book model) => await _service.Create(model);

        /// <summary>
        /// Редактирование книги
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<Guid> Update(
            [FromRoute] Guid id,
            [FromBody] Book model) => await _service.Update(id, model);

        /// <summary>
        /// Удаление книги по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] Guid id) => await _service.Delete(id);

        /// <summary>
        /// Сохранение картотеки в файл
        /// </summary>
        /// <returns></returns>
        [HttpGet("Download")]
        public async Task<FileStreamResult> Download()
        {
            var json = await _service.GetJson();

            var stream = GenerateStreamFromString(json);
            return new FileStreamResult(stream, MediaTypeNames.Application.Octet);

            static MemoryStream GenerateStreamFromString(string s)
            {
                var byteArray = Encoding.ASCII.GetBytes(s);
                var stream = new MemoryStream(byteArray);

                return stream;
            }
        }

        /// <summary>
        /// Загрузка картотеки из файла
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("Upload")]
        public async Task Upload(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var json = "";
            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                json = await reader.ReadToEndAsync();
            }

            await _service.SaveFromJson(json);
        }
    }
}