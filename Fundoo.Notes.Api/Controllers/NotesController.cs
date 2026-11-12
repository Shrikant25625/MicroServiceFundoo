using Fundoo.Notes.Api.Models;
using Fundoo.Notes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Fundoo.Notes.Api.Controllers
{
    [ApiController]
    [Route("api/notes")]
    public class NotesController(INoteApplication noteApplication, IDistributedCache cache) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetNotes([FromQuery] string userId, CancellationToken ct)
        {
            string cacheKey = $"notes_{userId}";

            // 1. Check Redis Cache first
            var cachedNotes = await cache.GetStringAsync(cacheKey, ct);
            if (!string.IsNullOrEmpty(cachedNotes))
            {
                var notesList = JsonSerializer.Deserialize<object>(cachedNotes);
                return Ok(notesList);
            }

            // 2. Cache Miss - Fetch from Database via Application layer
            var notes = await noteApplication.GetUserNotesAsync(userId, ct);

            // 3. Save to Redis Cache (Expires in 5 minutes)
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(notes), cacheOptions, ct);

            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequestModel model, CancellationToken ct)
        {
            var note = await noteApplication.CreateNoteAsync(model.UserId, model.Title, model.Description, model.Color, ct);

            // Invalidate Cache on Create
            await cache.RemoveAsync($"notes_{model.UserId}", ct);

            return Ok(new { message = "Note created successfully", note });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] UpdateNoteRequestModel model, CancellationToken ct)
        {
            var updated = await noteApplication.UpdateNoteAsync(id, model.UserId, model.Title, model.Description, model.Color, model.IsPinned, model.IsArchived, ct);
            if (updated == null) return NotFound(new { message = "Note not found" });

            // Invalidate Cache on Update
            await cache.RemoveAsync($"notes_{model.UserId}", ct);

            return Ok(new { message = "Note updated successfully", note = updated });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteNote(Guid id, [FromQuery] string userId, CancellationToken ct)
        {
            var success = await noteApplication.DeleteNoteAsync(id, userId, ct);
            if (!success) return NotFound(new { message = "Note not found" });

            // Invalidate Cache on Delete
            await cache.RemoveAsync($"notes_{userId}", ct);

            return Ok(new { message = "Note deleted successfully" });
        }
    }
}