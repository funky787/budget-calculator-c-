using BudgetCalculator.Api.Controllers;
using BudgetCalculator.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BudgetCalculator.Tests;

public class BudgetControllerTests
{
    private readonly BudgetController _controller = new();

    [Fact]
    public void Calculate_WithoutInsurance_Returns500()
    {
        var result = _controller.Calculate(new BudgetRequest
        {
            DailyCost = 100m,
            Days = 5,
            IncludeInsurance = false
        });

        var response = Assert.IsType<OkObjectResult>(result);
        var body = Assert.IsType<BudgetResponse>(response.Value);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal(500m, body.TotalAmount);
    }

    [Fact]
    public void Calculate_WithInsurance_AddsFivePercent()
    {
        var result = _controller.Calculate(new BudgetRequest
        {
            DailyCost = 100m,
            Days = 5,
            IncludeInsurance = true
        });

        var response = Assert.IsType<OkObjectResult>(result);
        var body = Assert.IsType<BudgetResponse>(response.Value);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal(525m, body.TotalAmount);
    }

    [Fact]
    public void Calculate_NegativeDays_Returns400()
    {
        var result = _controller.Calculate(new BudgetRequest
        {
            DailyCost = 100m,
            Days = -1
        });

        var response = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, response.StatusCode);
    }

    [Fact]
    public void Calculate_ZeroDailyCost_ReturnsZero()
    {
        var result = _controller.Calculate(new BudgetRequest
        {
            DailyCost = 0m,
            Days = 5
        });

        var response = Assert.IsType<OkObjectResult>(result);
        var body = Assert.IsType<BudgetResponse>(response.Value);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal(0m, body.TotalAmount);
    }

    [Fact]
    public void Calculate_FractionalDailyCost_KeepsKopecks()
    {
        var result = _controller.Calculate(new BudgetRequest
        {
            DailyCost = 100.50m,
            Days = 3
        });

        var response = Assert.IsType<OkObjectResult>(result);
        var body = Assert.IsType<BudgetResponse>(response.Value);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal(301.50m, body.TotalAmount);
    }

    [Fact]
    public void Calculate_ZeroDays_Returns400()
    {
        var result = _controller.Calculate(new BudgetRequest
        {
            DailyCost = 100m,
            Days = 0
        });

        var response = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, response.StatusCode);
    }
}
