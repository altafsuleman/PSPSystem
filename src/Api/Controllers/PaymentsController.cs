using HumbleMediator;
using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Application.Payments.Commands;
using WebApiTemplate.Application.Payments.Queries;
using WebApiTemplate.Core.Models.Transaction;

namespace WebApiTemplate.Api.Controllers;

public sealed class PaymentsController : AppControllerBase
{
    public PaymentsController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<TransactionResult>> Create(CreatePaymentCommand request)
    {
        var result = await _mediator.SendCommand<CreatePaymentCommand, TransactionResult>(request);
        return CreatedAtAction(nameof(Create), result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<TransactionResult>> GetById(string id)
    {
        var result = await _mediator.SendQuery<GetPaymentByIdQuery, TransactionResult>(
            new GetPaymentByIdQuery(id)
        );
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
