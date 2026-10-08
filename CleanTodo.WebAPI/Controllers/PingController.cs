using CleanTodo.Application.Service;
using CleanTodo.Application.UseCases;
using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PingController() : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("/api/ping")]

    public IActionResult Ping()
    {
        return Ok(new { message = "Pong" });
    }
}