using Microsoft.AspNetCore.Mvc;
using PaymentsBackend.DataModels;
using PaymentsBackend.DataStore;

[ApiController]
[Route("api/[controller]/[action]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentStore _store;

    public PaymentsController(IPaymentStore store)
    {
        _store = store;
    }

    [HttpGet]
    public async Task<IActionResult> Get( string? userId)
    {
        var response = string.IsNullOrEmpty(userId)
            ? await _store.GetAllPaymentsAsync()
            : await _store.GetUserPaymentsAsync(userId);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetById( string userId, int id)
    {
        var response = await _store.GetPaymentByIdAsync(userId, id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaymentRequestDto request)
    {
        var response = await _store.CreatePaymentAsync(request);
        return Ok(response);
    }

    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PaymentRequestDto request)
    {
        var response = await _store.UpdatePaymentAsync(request);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string userId, [FromQuery] int id)
    {
        var response = await _store.DeletePaymentAsync(userId, id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var response = await _store.GetAllUsersAsync();
        return Ok(response);
    }
}
