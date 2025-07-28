using AccountService.App.Commands;
using AccountService.App.DTO;
using AccountService.App.Queries;
using Common.Src;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers;

[Route("wallets")]
[ApiController]
[Produces("application/json")]
public class WalletsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Создать новый счёт
    /// </summary>
    /// <param name="command">Данные для создания счёта</param>
    /// <response code="201">Счёт успешно создан</response>
    /// <response code="400">Ошибка валидации</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateWalletCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    ///     Изменить счёт (процентная ставка)
    ///     Удалить счёт (дата закрытия)
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWalletCommand command)
    {
        if (id != command.WalletId)
            return BadRequest("ID в URL и команде не совпадают.");

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///     Получить список счетов.
    ///     Получить список у клиента
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<WalletDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] Guid? ownerId)
    {
        var wallets = await _mediator.Send(new GetWalletsQuery(ownerId));
        return Ok(wallets);
    }

    /// <summary>
    ///     Получить выписку по счёту за период
    /// </summary>
    /// <param name="walletId">ID счёта</param>
    /// <param name="from">Начало периода</param>
    /// <param name="to">Конец периода</param>
    [HttpGet("statement/{walletId}")]
    [ProducesResponseType(typeof(StatementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStatement(Guid walletId, [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var statement = await _mediator.Send(new GetStatementQuery(walletId, from, to));
        return Ok(statement);
    }

    /// <summary>
    ///     Зарегистрировать транзакцию по счёту (пополнение, списание)
    /// </summary>
    [HttpPost("transaction")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterTransaction([FromBody] RegisterTransactionCommand command)
    {
        var transactionId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = command.AccountId }, transactionId);
    }

    /// <summary>
    ///     Перевод между счетами
    /// </summary>
    [HttpPost("transfer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Transfer([FromBody] TransferBetweenWalletsCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    ///     Проверить, есть ли у клиента счёт определённого типа
    /// </summary>
    [HttpGet("exists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> Exists([FromQuery] Guid ownerId, [FromQuery] TypeWallet type)
    {
        var exists = await _mediator.Send(new CheckWalletExistsQuery(ownerId, type));
        return Ok(exists);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WalletDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var wallet = await _mediator.Send(new GetWalletByIdQuery(id));
        return wallet == null ? NotFound() : Ok(wallet);
    }
}