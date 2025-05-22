using SmartSchool.BAL;
using SmartSchool.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Advocate_Invoceing.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

     
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return View(clients);
        }

      
        public async Task<IActionResult> Details(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

       
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(ClientEntity client)
        {
            if (ModelState.IsValid)
            {
                await _clientService.AddClientAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

       
        [HttpPost] 
        public async Task<IActionResult> Edit(int id, ClientEntity client)
        {
            if (id != client.ClientId) return NotFound();

            if (ModelState.IsValid)
            {
                var updated = await _clientService.UpdateClientAsync(client);
                if (updated == null) return NotFound();

                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

   
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
