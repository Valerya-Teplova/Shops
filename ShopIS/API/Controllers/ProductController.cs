using System;
using DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class ProductController : ApiController
    {

        [HttpGet]
        [System.Web.Http.Route("api/Products/All")]
        public IHttpActionResult Get()
        {
            using (StoreEntities entities = new StoreEntities())
            {
                 
                var results = entities.Product.ToList();

                //if (results == null)
                //{
                //    return NotFound();
                //}

                return Ok(results);
            }
        }
       
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            try
            {
                using (StoreEntities entities = new StoreEntities())
                {
                    var emp = entities.Product.First(em => em.IDProduct == id);
                    if (emp != null)
                    {
                        return Ok(emp);
                    }
                    else return Content(HttpStatusCode.NotFound, "Product with Id: " + id + " not found");
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);

            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] Product product)
        {
            try
            {
                using (StoreEntities entities = new StoreEntities())
                {
                    entities.Product.Add(product);
                    entities.SaveChanges();
                    var res = Request.CreateResponse(HttpStatusCode.Created, product);
                    res.Headers.Location = new Uri(Request.RequestUri + product.IDProduct.ToString());
                    return res;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody] Product emp)
        {
            try
            {
                using (StoreEntities entities = new StoreEntities())
                {
                    var product = entities.Product.Where(em => em.IDProduct == id).First();
                    if (product != null)
                    {
                        if (!string.IsNullOrWhiteSpace(emp.ProductName))
                            product.ProductName = emp.ProductName;
                        if (!string.IsNullOrWhiteSpace(emp.Price+""))
                            product.Price = emp.Price;
                        if(!string.IsNullOrWhiteSpace(emp.Category))
                            product.Category = emp.Category;
                        if(!string.IsNullOrWhiteSpace(emp.Count+""))
                            product.Count = emp.Count;
                        if (!string.IsNullOrWhiteSpace(emp.Description))
                            product.Description = emp.Description;
                        

                        entities.SaveChanges();
                        var res = Request.CreateResponse(HttpStatusCode.OK, "product with id " + id + " updated");
                        res.Headers.Location = new Uri(Request.RequestUri + id.ToString());
                        return res;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with id " + id + " is not found!");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (StoreEntities entities = new StoreEntities())
                {
                    var product = entities.Product.Where(emp => emp.IDProduct == id).First();
                    if (product != null)
                    {
                        entities.Product.Remove(product);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Product with id " + id + " Deleted");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with id" + id + " is not found!");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
