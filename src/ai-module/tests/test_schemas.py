import pytest
from app.schemas.ai_schemas import (
    ExplanationRequest,
    ProblemRequest,
    HintRequest,
    SubjectType,
    DifficultyLevel
)


def test_explanation_request_valid():
    request = ExplanationRequest(
        topic="Ecuații de gradul 2",
        subject=SubjectType.MATHEMATICS,
        difficulty_level=DifficultyLevel.BEGINNER,
        student_age=14
    )
    assert request.topic == "Ecuații de gradul 2"
    assert request.subject == SubjectType.MATHEMATICS
    assert request.difficulty_level == DifficultyLevel.BEGINNER


def test_explanation_request_invalid_age():
    with pytest.raises(Exception):
        ExplanationRequest(
            topic="Test",
            subject=SubjectType.MATHEMATICS,
            difficulty_level=DifficultyLevel.BEGINNER,
            student_age=3
        )


def test_problem_request_valid():
    request = ProblemRequest(
        topic="Funcții",
        subject=SubjectType.MATHEMATICS,
        difficulty_level=DifficultyLevel.INTERMEDIATE,
        student_age=16,
        count=3
    )
    assert request.count == 3


def test_problem_request_invalid_count():
    with pytest.raises(Exception):
        ProblemRequest(
            topic="Test",
            subject=SubjectType.MATHEMATICS,
            difficulty_level=DifficultyLevel.BEGINNER,
            student_age=14,
            count=10
        )


def test_hint_request_valid():
    request = HintRequest(
        question="Ce este o ecuație de gradul 2?",
        subject=SubjectType.MATHEMATICS
    )
    assert request.question == "Ce este o ecuație de gradul 2?"
