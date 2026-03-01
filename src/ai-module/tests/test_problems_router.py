import pytest
from unittest.mock import AsyncMock, patch
from fastapi.testclient import TestClient
from app.main import app
from app.schemas.ai_schemas import (
    ProblemResponse,
    SubjectType,
    DifficultyLevel
)

client = TestClient(app)


def test_create_problems_mock():
    mock_response = ProblemResponse(
        topic="Funcții liniare",
        problems=["Rezolvați: 2x + 3 = 7", "Rezolvați: x - 5 = 10"],
        hints=["Izolați x", "Mutați termenii"],
        solutions=["x = 2", "x = 15"],
        subject=SubjectType.MATHEMATICS,
        difficulty_level=DifficultyLevel.BEGINNER
    )

    with patch("app.services.ai_service.generate_problems",
               new_callable=AsyncMock) as mock_gen:
        mock_gen.return_value = mock_response

        response = client.post("/api/problems", json={
            "topic": "Funcții liniare",
            "subject": 1,
            "difficulty_level": 1,
            "student_age": 14,
            "count": 2
        })

        assert response.status_code == 200
        data = response.json()
        assert data["topic"] == "Funcții liniare"
        assert "problems" in data
        assert "hints" in data
        assert "solutions" in data


def test_create_problems_invalid_request():
    response = client.post("/api/problems", json={
        "topic": "",
        "subject": 1,
        "difficulty_level": 1,
        "student_age": 14,
        "count": 1
    })
    assert response.status_code == 422
