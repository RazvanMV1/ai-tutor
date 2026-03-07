using AiTutor.Web.Models;

namespace AiTutor.Web.Store.Subjects;

public record LoadSubjectsAction;
public record LoadSubjectsSuccessAction(List<SubjectDto> Subjects);
public record LoadSubjectsFailureAction(string Error);
public record LoadLessonsSuccessAction(List<LessonDto> Lessons);
