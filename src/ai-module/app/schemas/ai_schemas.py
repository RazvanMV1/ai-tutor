from pydantic import BaseModel, Field
from enum import IntEnum


class DifficultyLevel(IntEnum):
    BEGINNER = 1
    INTERMEDIATE = 2
    ADVANCED = 3


class SubjectType(IntEnum):
    MATHEMATICS = 1
    ROMANIAN = 2
    INFORMATICS = 3


class ExplanationRequest(BaseModel):
    topic: str = Field(..., min_length=1, max_length=500)
    subject: SubjectType
    difficulty_level: DifficultyLevel
    student_age: int = Field(default=14, ge=6, le=18)


class ExplanationResponse(BaseModel):
    topic: str
    explanation: str
    examples: list[str]
    key_points: list[str]
    subject: SubjectType
    difficulty_level: DifficultyLevel


class ProblemRequest(BaseModel):
    topic: str = Field(..., min_length=1, max_length=500)
    subject: SubjectType
    difficulty_level: DifficultyLevel
    student_age: int = Field(default=14, ge=6, le=18)
    count: int = Field(default=1, ge=1, le=5)


class ProblemResponse(BaseModel):
    topic: str
    problems: list[str]
    hints: list[str]
    solutions: list[str]
    subject: SubjectType
    difficulty_level: DifficultyLevel


class HintRequest(BaseModel):
    question: str = Field(..., min_length=1, max_length=1000)
    subject: SubjectType


class HintResponse(BaseModel):
    question: str
    hint: str
    subject: SubjectType


# ===== Lesson Chat =====
class ChatMessage(BaseModel):
    role: str  # "user" sau "assistant"
    content: str


class LessonChatRequest(BaseModel):
    lesson_title: str = Field(..., min_length=1, max_length=300)
    lesson_content: str = Field(default="", max_length=20000)
    subject: SubjectType
    difficulty_level: DifficultyLevel
    question: str = Field(..., min_length=1, max_length=2000)
    history: list[ChatMessage] = Field(default_factory=list)


class LessonChatResponse(BaseModel):
    answer: str
