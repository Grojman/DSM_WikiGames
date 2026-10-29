using WikiGames.Application.Services;
using WikiGames.Application.UseCases.Valoracion;
using WikiGames.Domain.Entities;
using Xunit.Abstractions;
using WikiGames.Domain.Enums;

namespace WikiGames.Tests.Manual;

public sealed class ManualOperationTests
{
    private readonly ITestOutputHelper _output;

    public ManualOperationTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Usuario_ActualizarRol_Custom_Manual_Contract()
    {
        // <haria-protected id="test:custom:Usuario:actualizarRol:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "actualizarRol";
        var operationType = "Custom";
        var executeManualInvocation = false;
        UsuarioService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:custom:Usuario:actualizarRol:invoke">
        await ((dynamic)service!).ActualizarRolAsync(default(int), default(int), Rol.Registrado);
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:custom:Usuario:actualizarRol:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Videojuego_AceptarVideojuego_Custom_Manual_Contract()
    {
        // <haria-protected id="test:custom:Videojuego:aceptarVideojuego:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "aceptarVideojuego";
        var operationType = "Custom";
        var executeManualInvocation = false;
        VideojuegoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:custom:Videojuego:aceptarVideojuego:invoke">
        await ((dynamic)service!).AceptarVideojuegoAsync(default(int), default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:custom:Videojuego:aceptarVideojuego:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Valoracion_NewValorValidado_Customized_Manual_Contract()
    {
        // <haria-protected id="test:customized:Valoracion:newValorValidado:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "newValorValidado";
        var operationType = "New";
        var executeManualInvocation = false;
        ValoracionService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customized:Valoracion:newValorValidado:invoke">
        invocationResult = await ((dynamic)service!).NewValorValidadoAsync("Sample Valoracion Texto", new DateTime(2026, 1, 1), 1, 1.5f, default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customized:Valoracion:newValorValidado:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Valoracion_ActualizarMediaVideojuego_CustomTransaction_Manual_Contract()
    {
        // <haria-protected id="test:customTransaction:Valoracion:actualizarMediaVideojuego:arrange">
        // Create a test unit of work and assign useCase before enabling the generated invocation.
        // </haria-protected>

        var operationName = "actualizarMediaVideojuego";
        var operationType = "CustomTransaction";
        var executeManualInvocation = false;
        ValoracionUseCase? useCase = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customTransaction:Valoracion:actualizarMediaVideojuego:invoke">
        await ((dynamic)useCase!).ActualizarMediaVideojuegoAsync(default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customTransaction:Valoracion:actualizarMediaVideojuego:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task AccionSistema_NewUsuarioValido_Customized_Manual_Contract()
    {
        // <haria-protected id="test:customized:AccionSistema:newUsuarioValido:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "newUsuarioValido";
        var operationType = "New";
        var executeManualInvocation = false;
        AccionSistemaService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customized:AccionSistema:newUsuarioValido:invoke">
        invocationResult = await ((dynamic)service!).NewUsuarioValidoAsync(new DateTime(2026, 1, 1), "Sample AccionSistema Descripcion", default(int), default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customized:AccionSistema:newUsuarioValido:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Rechazo_NewUsuarioValido_Customized_Manual_Contract()
    {
        // <haria-protected id="test:customized:Rechazo:newUsuarioValido:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "newUsuarioValido";
        var operationType = "New";
        var executeManualInvocation = false;
        RechazoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customized:Rechazo:newUsuarioValido:invoke">
        invocationResult = await ((dynamic)service!).NewUsuarioValidoAsync(new DateTime(2026, 1, 1), "Sample Rechazo Motivo", default(int), default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customized:Rechazo:newUsuarioValido:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Contenido_DarMeGusta_Custom_Manual_Contract()
    {
        // <haria-protected id="test:custom:Contenido:darMeGusta:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "darMeGusta";
        var operationType = "Custom";
        var executeManualInvocation = false;
        ContenidoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:custom:Contenido:darMeGusta:invoke">
        await ((dynamic)service!).DarMeGustaAsync(default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:custom:Contenido:darMeGusta:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Contenido_RetirarMeGusta_Custom_Manual_Contract()
    {
        // <haria-protected id="test:custom:Contenido:retirarMeGusta:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "retirarMeGusta";
        var operationType = "Custom";
        var executeManualInvocation = false;
        ContenidoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:custom:Contenido:retirarMeGusta:invoke">
        await ((dynamic)service!).RetirarMeGustaAsync(default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:custom:Contenido:retirarMeGusta:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    private const string SuccessColor = "\u001b[32m";
    private const string ErrorColor = "\u001b[31m";
    private const string ResetColor = "\u001b[0m";

    private void EnsureContractOutput(string operationName, string operationType)
    {
        Ensure(!string.IsNullOrWhiteSpace(operationName), $"{operationName} declares the manual operation name.");
        Ensure(!string.IsNullOrWhiteSpace(operationType), $"{operationName} declares the manual operation type.");
        WriteTestMessage(true, $"{operationName} contract uses operation type {operationType}.");
    }

    private void Ensure(bool condition, string message)
    {
        WriteTestMessage(condition, message);
        Assert.True(condition, message);
    }

    private void WriteTestMessage(bool condition, string message)
    {
        var color = condition ? SuccessColor : ErrorColor;
        var status = condition ? "[OK] " : "[FAIL] ";
        _output.WriteLine($"{color}{status}{message}{ResetColor}");
    }

}
