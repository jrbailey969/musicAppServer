using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Domain.Service;
using MusicApp.Domain.Entities.Classification;
using MusicApp.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class ClassificationCategoryController : Controller
    {
        private readonly ClassificationCategoryService _categoryService;

        public ClassificationCategoryController(ClassificationCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<List<ClassificationCategory>> Get()
        {
            return await _categoryService.GetByUserId("0e54b6da-caef-492f-bc95-b0669a8957d5");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ClassificationCategory> Get(string id)
        {
            return await _categoryService.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public async Task<ClassificationCategory> Post([FromBody]ClassificationCategory category)
        {
            var newCategory = _categoryService.CreateNew(new User { Id = "0e54b6da-caef-492f-bc95-b0669a8957d5" });
            MapValues(newCategory, category);
            await _categoryService.AddOrUpdate(newCategory);

            return newCategory;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody]ClassificationCategory category)
        {
            var existingCategory = await _categoryService.GetById(id);
            MapValues(existingCategory, category);
            await _categoryService.Update(existingCategory);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var category = await _categoryService.GetById(id);

            if (category != null)
            {
                await _categoryService.Delete(category);
            }
        }

        private void MapValues(ClassificationCategory target, ClassificationCategory source)
        {
            target.Name = source.Name;
            target.ClassificationCategoryTypeId = source.ClassificationCategoryTypeId;
            target.RangeMax = source.RangeMax;
            target.RangeMin = source.RangeMin;

            target.Tags = target.Tags ?? new List<ClassificationTag>();


            // Remove tags that are not in source
            for (int i = target.Tags.Count - 1; i >= 0; i--)
            {
                var tag = target.Tags[i];
                if (!source.Tags.Where(t => t.Id == tag.Id).Any())
                {
                    target.Tags.RemoveAt(i);
                }
            }

            // Map Tags
            foreach (ClassificationTag sourceTag in source.Tags)
            {
                ClassificationTag targetTag = target.Tags.Where(t => !string.IsNullOrWhiteSpace(sourceTag.Id) && t.Id == sourceTag.Id).SingleOrDefault();

                if (targetTag == null)
                {
                    targetTag = _categoryService.CreateNewTag();
                    target.Tags.Add(targetTag);
                }
                else
                {
                    targetTag.ModifiedDate = DateTime.UtcNow;
                }

                targetTag.Name = sourceTag.Name;
            }

            target.Tags.Sort();
        }
    }
}
