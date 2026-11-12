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
        /*PROTECTED REGION ID(test:customized:Videojuego:newCustomized:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "newCustomized";
        var operationType = "New";
        var executeManualInvocation = false;
        VideojuegoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:customized:Videojuego:newCustomized:invoke) ENABLED START*/
        invocationResult = await ((dynamic)service!).NewCustomizedAsync("Sample Videojuego Texto", new DateTime(2026, 1, 1), "Sample Videojuego Titulo", "Sample Videojuego Enlace", "Sample Videojuego Imagen", default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:customized:Videojuego:newCustomized:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Videojuego_AprobarVideojuego_Custom_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:custom:Videojuego:aprobarVideojuego:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "aprobarVideojuego";
        var operationType = "Custom";
        var executeManualInvocation = false;
        VideojuegoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:custom:Videojuego:aprobarVideojuego:invoke) ENABLED START*/
        await ((dynamic)service!).AprobarVideojuegoAsync(default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:custom:Videojuego:aprobarVideojuego:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Videojuego_RechazarVideojuego_Custom_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:custom:Videojuego:rechazarVideojuego:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "rechazarVideojuego";
        var operationType = "Custom";
        var executeManualInvocation = false;
        VideojuegoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:custom:Videojuego:rechazarVideojuego:invoke) ENABLED START*/
        await ((dynamic)service!).RechazarVideojuegoAsync(default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:custom:Videojuego:rechazarVideojuego:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Categoria_NewCustomized_Customized_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:customized:Categoria:newCustomized:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "newCustomized";
        var operationType = "New";
        var executeManualInvocation = false;
        CategoriaService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:customized:Categoria:newCustomized:invoke) ENABLED START*/
        invocationResult = await ((dynamic)service!).NewCustomizedAsync("Sample Categoria Name", default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:customized:Categoria:newCustomized:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Denuncia_NewCustomized_Customized_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:customized:Denuncia:newCustomized:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "newCustomized";
        var operationType = "New";
        var executeManualInvocation = false;
        DenunciaService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:customized:Denuncia:newCustomized:invoke) ENABLED START*/
        invocationResult = await ((dynamic)service!).NewCustomizedAsync(new DateTime(2026, 1, 1), default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:customized:Denuncia:newCustomized:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Denuncia_AprobarDenuncia_Custom_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:custom:Denuncia:aprobarDenuncia:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "aprobarDenuncia";
        var operationType = "Custom";
        var executeManualInvocation = false;
        DenunciaService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:custom:Denuncia:aprobarDenuncia:invoke) ENABLED START*/
        await ((dynamic)service!).AprobarDenunciaAsync(default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:custom:Denuncia:aprobarDenuncia:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Denuncia_RechazarDenuncia_Custom_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:custom:Denuncia:rechazarDenuncia:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "rechazarDenuncia";
        var operationType = "Custom";
        var executeManualInvocation = false;
        DenunciaService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:custom:Denuncia:rechazarDenuncia:invoke) ENABLED START*/
        await ((dynamic)service!).RechazarDenunciaAsync(default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:custom:Denuncia:rechazarDenuncia:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Valoracion_ActualizarValoracion_CustomTransaction_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:customTransaction:Valoracion:actualizarValoracion:arrange) ENABLED START*/
        // Create a test unit of work and assign useCase before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "actualizarValoracion";
        var operationType = "CustomTransaction";
        var executeManualInvocation = false;
        ValoracionUseCase? useCase = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:customTransaction:Valoracion:actualizarValoracion:invoke) ENABLED START*/
        await ((dynamic)useCase!).ActualizarValoracionAsync(default(int), 1.5f);
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:customTransaction:Valoracion:actualizarValoracion:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Valoracion_NuevaValoracion_CustomTransaction_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:customTransaction:Valoracion:nuevaValoracion:arrange) ENABLED START*/
        // Create a test unit of work and assign useCase before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "nuevaValoracion";
        var operationType = "CustomTransaction";
        var executeManualInvocation = false;
        ValoracionUseCase? useCase = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:customTransaction:Valoracion:nuevaValoracion:invoke) ENABLED START*/
        await ((dynamic)useCase!).NuevaValoracionAsync(default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:customTransaction:Valoracion:nuevaValoracion:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Valoracion_EliminarValoracion_CustomTransaction_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:customTransaction:Valoracion:eliminarValoracion:arrange) ENABLED START*/
        // Create a test unit of work and assign useCase before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "eliminarValoracion";
        var operationType = "CustomTransaction";
        var executeManualInvocation = false;
        ValoracionUseCase? useCase = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:customTransaction:Valoracion:eliminarValoracion:invoke) ENABLED START*/
        await ((dynamic)useCase!).EliminarValoracionAsync(default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:customTransaction:Valoracion:eliminarValoracion:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Contenido_NewCustomized_Customized_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:customized:Contenido:newCustomized:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "newCustomized";
        var operationType = "New";
        var executeManualInvocation = false;
        ContenidoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:customized:Contenido:newCustomized:invoke) ENABLED START*/
        invocationResult = await ((dynamic)service!).NewCustomizedAsync("Sample Contenido Texto", new DateTime(2026, 1, 1), default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:customized:Contenido:newCustomized:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Contenido_DarMeGusta_Custom_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:custom:Contenido:darMeGusta:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "darMeGusta";
        var operationType = "Custom";
        var executeManualInvocation = false;
        ContenidoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:custom:Contenido:darMeGusta:invoke) ENABLED START*/
        await ((dynamic)service!).DarMeGustaAsync(default(int), default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:custom:Contenido:darMeGusta:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
    }

    [Fact]
    public async Task Contenido_RetirarMeGusta_Custom_Manual_Contract()
    {
        /*PROTECTED REGION ID(test:custom:Contenido:retirarMeGusta:arrange) ENABLED START*/
        // Create a test unit of work and assign service before enabling the generated invocation.
        /*PROTECTED REGION END*/

        var operationName = "retirarMeGusta";
        var operationType = "Custom";
        var executeManualInvocation = false;
        ContenidoService? service = null;
        object? invocationResult = null;

        if (executeManualInvocation)
        {
        /*PROTECTED REGION ID(test:custom:Contenido:retirarMeGusta:invoke) ENABLED START*/
        await ((dynamic)service!).RetirarMeGustaAsync(default(int), default(int));
        /*PROTECTED REGION END*/
        }
        _ = invocationResult;

        /*PROTECTED REGION ID(test:custom:Contenido:retirarMeGusta:assert) ENABLED START*/
        EnsureContractOutput(operationName, operationType);
        /*PROTECTED REGION END*/
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
