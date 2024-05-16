using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AddressAce.Client.Services.Interfaces;
using AddressAce.Client.Models;
using AddressAce.Helpers.Extensions;
using AddressAce.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContactsController : ControllerBase
{
    private readonly IContactDTOService _contactService;
    private string _userId => User.GetUserId()!; // [authorize] means userId cannot be null

    public ContactsController(IContactDTOService contactService)
    {
        _contactService = contactService;
    }

    // GET: "api/contacts" OR "api/contacts?categoryId=4" -> list of user contacts, optionally filtered by category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContacts([FromQuery] int? categoryId)
    {
        try
        {
            IEnumerable<ContactDTO> contacts;

            if (categoryId != null)
            {
                contacts = await _contactService.GetContactsByCategoryIdAsync(categoryId.Value, _userId);
            }
            else
            {
                contacts = await _contactService.GetContactsAsync(_userId);
            }
            return Ok(contacts);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }


    // GET: "api/contacts/5" -> a contact or 404
    [HttpGet("{contactId}")]
    public  async Task<ActionResult<ContactDTO?>> GetContactById([FromRoute] int contactId)
    {

        try
        {
            ContactDTO? contact = await _contactService.GetContactByIdAsync(contactId, _userId);
            if (contact == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(contact);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    //GET: "api/contacts/search?query=whatever" -> contacts matching the search query

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ContactDTO>>> SearchContacts([FromQuery] string searchTerm)
    {
        try
        {
            IEnumerable<ContactDTO> contacts = await _contactService.SearchContactsAsync(searchTerm, _userId);
            return Ok(contacts);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    // POST: "api/contacts" -> creates and returns the created contact
    [HttpPost]
    public async Task<ActionResult<ContactDTO>> CreateContact([FromBody] ContactDTO newContact)
    {
        try
        {
            ContactDTO contact = await _contactService.CreateContactAsync(newContact, _userId);
            return Ok(contact);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    // PUT: "api/contacts/5" -> updates the selected contact and returns Ok
    [HttpPut("{Id:int}")]
    public async Task<ActionResult> UpdateContact([FromRoute] int Id, [FromBody] ContactDTO contactDTO)
    {
        if ( Id != contactDTO.Id)
            return BadRequest("ID mismatch");

        await _contactService.UpdateContactAsync(contactDTO, _userId);
        return Ok();
    }

    //// DELETE: "api/contacts/5" -> deletes the selected contact and returns NoContent
    [HttpDelete("{contactId}")]
    public async Task<ActionResult<ContactDTO>> DeleteContact([FromRoute] int contactId)
    {
        try
        {
            await _contactService.DeleteContactAsync(contactId, _userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    //// POST: "api/contacts/5/email" -> sends an email to contact and returns Ok or BadRequest to indicate success or failure

    [HttpPost("{contactId}/email")]
    public async Task<ActionResult> EmailContact([FromRoute] int contactId, [FromBody] EmailData emailData)
    {
        try
        {
            bool result = await _contactService.EmailContactAsync(contactId, emailData, _userId);
            if (result == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to send email.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }
}