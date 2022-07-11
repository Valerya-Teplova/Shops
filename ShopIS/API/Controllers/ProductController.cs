using System;
using DataAccessLayer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class ProductController : ApiController
    {

        [System.Web.Http.Route("api/Product")]
        [HttpGet]
        public IHttpActionResult Get(int id = -1)
        {
            try
            {
                using (StoreEntities entities = new StoreEntities())
                {
                    if (id == -1)
                    {
                        var results = entities.Product.ToList();

                        if (results == null)
                        {
                            return NotFound();
                        }

                        return Ok(results);
                    }
                    else
                    {
                        var product = entities.Product.First(em => em.IDProduct == id);
                        if (product != null)
                        {
                            return Ok(product);
                        }
                        else return Content(HttpStatusCode.NotFound, "Product with Id: " + id + " not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);

            }
        }
        [System.Web.Http.Route("api/Product")]
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
        [System.Web.Http.Route("api/Product")]
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody] Product product)
        {
            try
            {
                using (StoreEntities entities = new StoreEntities())
                {
                    var result = entities.Product.Where(pr => pr.IDProduct == id).First();
                    if (result != null)
                    {
 
                        if (!string.IsNullOrWhiteSpace(product.ProductName))
                            product.ProductName = product.ProductName;                        
                        if (!string.IsNullOrWhiteSpace(product.Price.ToString()))                        
                            product.Price = product.Price;                        
                        if (!string.IsNullOrWhiteSpace(product.Category))
                            product.Category = product.Category;
                        if (!string.IsNullOrWhiteSpace(product.Count.ToString()))
                            product.Count = product.Count;
                        if (!string.IsNullOrWhiteSpace(product.Description))
                            product.Description = product.Description;
 

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

        [System.Web.Http.Route("api/Product")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id=0)
        {
            try
            {
                using (StoreEntities entities = new StoreEntities())
                {
                    var result = entities.Product.First(product => product.IDProduct == id);
                    if (result != null)
                    {
                        entities.Product.Remove(result);
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
