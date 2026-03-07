using AiTutor.Web.Models;

namespace AiTutor.Web.Store.Progress;

public record LoadProgressAction(Guid UserId);
public record LoadProgressSuccessAction(List<StudentProgressDto> Items);
public record LoadProgressFailureAction(string Error);
