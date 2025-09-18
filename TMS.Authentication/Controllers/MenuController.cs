using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using TMS.Authentication.Model;
using TMS.Authentication.Menu;
using Microsoft.AspNetCore.Hosting;
namespace TMS.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController :  ControllerBase
    {
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;


        public MenuController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        #region Super Admin Menu
        [HttpGet("SuperAdminMenu")]
        [Authorize]
        public IActionResult SuperAdminMenu()
        { 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Menus)); // Replace 'MenuType' with the actual class you're deserializing to
            string projectRootPath = _hostingEnvironment.ContentRootPath;
            string filePath = Path.Combine(projectRootPath, "Menu", "SuperAdminMenu.xml");
            //// Use 'using' to ensure the FileStream is disposed of
            using (FileStream xmlStream = new FileStream(filePath, FileMode.Open))
            {
                // Deserialize the XML stream to the object
                var result = xmlSerializer.Deserialize(xmlStream);

                // Return the result (you can change 'MenuType' as per your requirements)
                return Ok(new { result });
            }
        }

        #endregion
        #region Admin Menu
        [HttpGet("AdminMenu")]
        [Authorize]
        public IActionResult AdminMenu()
        { 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Menus)); // Replace 'MenuType' with the actual class you're deserializing to
            string projectRootPath = _hostingEnvironment.ContentRootPath;
            string filePath = Path.Combine(projectRootPath, "Menu", "AdminMenu.xml");
            //// Use 'using' to ensure the FileStream is disposed of
            using (FileStream xmlStream = new FileStream(filePath, FileMode.Open))
            {
                // Deserialize the XML stream to the object
                var result = xmlSerializer.Deserialize(xmlStream);

                // Return the result (you can change 'MenuType' as per your requirements)
                return Ok(new { result });
            }
        }

        #endregion
        #region Customer  Menu
        [HttpGet("CustomerMenu")]
        [Authorize]
        public IActionResult CustomerMenu()
        { 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Menus)); // Replace 'MenuType' with the actual class you're deserializing to
            string projectRootPath = _hostingEnvironment.ContentRootPath;
            string filePath = Path.Combine(projectRootPath, "Menu", "CustomerMenu.xml");
            //// Use 'using' to ensure the FileStream is disposed of
            using (FileStream xmlStream = new FileStream(filePath, FileMode.Open))
            {
                // Deserialize the XML stream to the object
                var result = xmlSerializer.Deserialize(xmlStream);

                // Return the result (you can change 'MenuType' as per your requirements)
                return Ok(new { result });
            }
        }

        #endregion
        #region Agent or Developer Menu
        [HttpGet("AgentMenu")]
        [Authorize]
        public IActionResult AgentMenu()
        { 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Menus));
            string projectRootPath = _hostingEnvironment.ContentRootPath;
            string filePath = Path.Combine(projectRootPath, "Menu", "AgentMenu.xml");
            //// Use 'using' to ensure the FileStream is disposed of
            using (FileStream xmlStream = new FileStream(filePath, FileMode.Open))
            {
                // Deserialize the XML stream to the object
                var result = xmlSerializer.Deserialize(xmlStream);

                // Return the result (you can change 'MenuType' as per your requirements)
                return Ok(new { result });
            }
        }



        
        #endregion
    }
}
