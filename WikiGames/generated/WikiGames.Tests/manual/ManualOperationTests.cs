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
    public async Task Videojuego_NewCustomized_Customized_Manual_Contract()
    {
        // <haria-protected id="test:customized:Videojuego:newCustomized:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "newCustomized";
        var operationType = "New";
        var executeManualInvocation = false;
        VideojuegoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customized:Videojuego:newCustomized:invoke">
        invocationResult = await ((dynamic)service!).NewCustomizedAsync("Sample Videojuego Texto", new DateTime(2026, 1, 1), "Sample Videojuego Titulo", "Sample Videojuego Enlace", "Sample Videojuego Imagen", default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customized:Videojuego:newCustomized:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Videojuego_AprobarVideojuego_Custom_Manual_Contract()
    {
        // <haria-protected id="test:custom:Videojuego:aprobarVideojuego:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "aprobarVideojuego";
        var operationType = "Custom";
        var executeManualInvocation = false;
        VideojuegoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:custom:Videojuego:aprobarVideojuego:invoke">
        await ((dynamic)service!).AprobarVideojuegoAsync(default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:custom:Videojuego:aprobarVideojuego:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Videojuego_RechazarVideojuego_Custom_Manual_Contract()
    {
        // <haria-protected id="test:custom:Videojuego:rechazarVideojuego:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "rechazarVideojuego";
        var operationType = "Custom";
        var executeManualInvocation = false;
        VideojuegoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:custom:Videojuego:rechazarVideojuego:invoke">
        await ((dynamic)service!).RechazarVideojuegoAsync(default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:custom:Videojuego:rechazarVideojuego:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Categoria_NewCustomized_Customized_Manual_Contract()
    {
        // <haria-protected id="test:customized:Categoria:newCustomized:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "newCustomized";
        var operationType = "New";
        var executeManualInvocation = false;
        CategoriaService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customized:Categoria:newCustomized:invoke">
        invocationResult = await ((dynamic)service!).NewCustomizedAsync("Sample Categoria Name", default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customized:Categoria:newCustomized:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Denuncia_NewCustomized_Customized_Manual_Contract()
    {
        // <haria-protected id="test:customized:Denuncia:newCustomized:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "newCustomized";
        var operationType = "New";
        var executeManualInvocation = false;
        DenunciaService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customized:Denuncia:newCustomized:invoke">
        invocationResult = await ((dynamic)service!).NewCustomizedAsync(new DateTime(2026, 1, 1), default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customized:Denuncia:newCustomized:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Denuncia_AprobarDenuncia_Custom_Manual_Contract()
    {
        // <haria-protected id="test:custom:Denuncia:aprobarDenuncia:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "aprobarDenuncia";
        var operationType = "Custom";
        var executeManualInvocation = false;
        DenunciaService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:custom:Denuncia:aprobarDenuncia:invoke">
        await ((dynamic)service!).AprobarDenunciaAsync(default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:custom:Denuncia:aprobarDenuncia:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Denuncia_RechazarDenuncia_Custom_Manual_Contract()
    {
        // <haria-protected id="test:custom:Denuncia:rechazarDenuncia:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "rechazarDenuncia";
        var operationType = "Custom";
        var executeManualInvocation = false;
        DenunciaService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:custom:Denuncia:rechazarDenuncia:invoke">
        await ((dynamic)service!).RechazarDenunciaAsync(default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:custom:Denuncia:rechazarDenuncia:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Valoracion_ActualizarValoracion_CustomTransaction_Manual_Contract()
    {
        // <haria-protected id="test:customTransaction:Valoracion:actualizarValoracion:arrange">
        // Create a test unit of work and assign useCase before enabling the generated invocation.
        // </haria-protected>

        var operationName = "actualizarValoracion";
        var operationType = "CustomTransaction";
        var executeManualInvocation = false;
        ValoracionUseCase? useCase = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customTransaction:Valoracion:actualizarValoracion:invoke">
        await ((dynamic)useCase!).ActualizarValoracionAsync(default(int), 1.5f);
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customTransaction:Valoracion:actualizarValoracion:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Valoracion_NuevaValoracion_CustomTransaction_Manual_Contract()
    {
        // <haria-protected id="test:customTransaction:Valoracion:nuevaValoracion:arrange">
        // Create a test unit of work and assign useCase before enabling the generated invocation.
        // </haria-protected>

        var operationName = "nuevaValoracion";
        var operationType = "CustomTransaction";
        var executeManualInvocation = false;
        ValoracionUseCase? useCase = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customTransaction:Valoracion:nuevaValoracion:invoke">
        await ((dynamic)useCase!).NuevaValoracionAsync(default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customTransaction:Valoracion:nuevaValoracion:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Valoracion_EliminarValoracion_CustomTransaction_Manual_Contract()
    {
        // <haria-protected id="test:customTransaction:Valoracion:eliminarValoracion:arrange">
        // Create a test unit of work and assign useCase before enabling the generated invocation.
        // </haria-protected>

        var operationName = "eliminarValoracion";
        var operationType = "CustomTransaction";
        var executeManualInvocation = false;
        ValoracionUseCase? useCase = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customTransaction:Valoracion:eliminarValoracion:invoke">
        await ((dynamic)useCase!).EliminarValoracionAsync(default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customTransaction:Valoracion:eliminarValoracion:assert">
        EnsureContractOutput(operationName, operationType);
        // </haria-protected>
    }

    [Fact]
    public async Task Contenido_NewCustomized_Customized_Manual_Contract()
    {
        // <haria-protected id="test:customized:Contenido:newCustomized:arrange">
        // Create a test unit of work and assign service before enabling the generated invocation.
        // </haria-protected>

        var operationName = "newCustomized";
        var operationType = "New";
        var executeManualInvocation = false;
        ContenidoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        // <haria-protected id="test:customized:Contenido:newCustomized:invoke">
        invocationResult = await ((dynamic)service!).NewCustomizedAsync("Sample Contenido Texto", new DateTime(2026, 1, 1), default(int));
        // </haria-protected>
        }
        _ = invocationResult;

        // <haria-protected id="test:customized:Contenido:newCustomized:assert">
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
        await ((dynamic)service!).DarMeGustaAsync(default(int), default(int));
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
        await ((dynamic)service!).RetirarMeGustaAsync(default(int), default(int));
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
