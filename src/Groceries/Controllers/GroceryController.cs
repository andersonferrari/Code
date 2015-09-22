using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Groceries.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Groceries.Controllers
{
    [Route("api/[controller]")]
    public class GroceryController : Controller
    {

        static readonly List<Grocery> _items = new List<Grocery>()
        {
            new Grocery { Id = 1, Title = "Red Apples", Price = 2.5M },
            new Grocery { Id = 2, Title = "Bananas", Price = 1 },
            new Grocery { Id = 3, Title = "French Bread", Price = 4.5M },
            new Grocery { Id = 4, Title = "New Zealand Brie Cheese", Price = 10.9M }
        };


        // GET: api/values
        [HttpGet]
        public IEnumerable<Grocery> Get()
        {
            return _items;
        }

        // GET api/values/5
        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return new ObjectResult(item);
        }

        // POST api/values
        [HttpPost]
        public Grocery Post([FromBody]Grocery item)
        {
            if (!ModelState.IsValid)
            {
                Context.Response.StatusCode = 400;
                return null;
            }
            else
            {
                item.Id = 1 + _items.Max(x => (int?)x.Id) ?? 0;
                _items.Add(item);

                string url = Url.RouteUrl("GetById", new { id = item.Id }, Request.Scheme, Request.Host.ToUriComponent());
                Context.Response.StatusCode = 201;
                Context.Response.Headers["Location"] = url;
                
                return item;

            }
        }

        // PUT api/values/5
        [HttpPut("{id:int}")]
        public void Put(int id, [FromBody]Grocery item)
        {
            if (!ModelState.IsValid)
            {
                Context.Response.StatusCode = 400;
            }
            else
            {
                int index = _items.IndexOf(_items.FirstOrDefault(g => g.Id == id));
                if (index != -1)
                    _items[index] = item;

                string url = Url.RouteUrl("GetById", new { id = item.Id }, Request.Scheme, Request.Host.ToUriComponent());
                Context.Response.StatusCode = 201;
                Context.Response.Headers["Location"] = url;
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            _items.Remove(item);
            return new HttpStatusCodeResult(204); // 201 No Content
        }

        // PUT api/values/5
        [HttpPost("{id:int}")]
        [Route("move/{id:int}")]
        public void Put(int id, [FromBody]bool directionDown) //1=true=down, 0=false=up
        {
            if (!ModelState.IsValid)
            {
                Context.Response.StatusCode = 400;
            }
            else
            {
                int oldIndex = _items.IndexOf(_items.FirstOrDefault(g => g.Id == id));
                int newIndex = -1;
                if (directionDown)
                {
                    if (oldIndex < (_items.Count() - 1))
                    { newIndex = oldIndex + 1; }
                } //up
                else
                {
                    if (oldIndex > 0)
                    { newIndex = oldIndex - 1; }

                }
                if (newIndex != -1)
                {
                    Grocery item = _items[oldIndex];
                    _items.RemoveAt(oldIndex);
                    _items.Insert(newIndex, item);
                }
            }
        }
    }
}
