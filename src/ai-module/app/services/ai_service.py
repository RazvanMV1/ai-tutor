import os
from openai import AsyncOpenAI
from app.schemas.ai_schemas import (
    ExplanationRequest, ExplanationResponse,
    ProblemRequest, ProblemResponse,
    HintRequest, HintResponse,
    SubjectType, DifficultyLevel
)

client = AsyncOpenAI(api_key=os.getenv("OPENAI_API_KEY", ""))

SUBJECT_NAMES = {
    SubjectType.MATHEMATICS: "Matematică",
    SubjectType.ROMANIAN: "Limba Română",
    SubjectType.INFORMATICS: "Informatică"
}

DIFFICULTY_NAMES = {
    DifficultyLevel.BEGINNER: "începător",
    DifficultyLevel.INTERMEDIATE: "intermediar",
    DifficultyLevel.ADVANCED: "avansat"
}


async def get_explanation(request: ExplanationRequest) -> ExplanationResponse:
    subject_name = SUBJECT_NAMES[request.subject]
    difficulty_name = DIFFICULTY_NAMES[request.difficulty_level]

    prompt = f"""Ești un profesor expert în {subject_name} pentru elevi de {request.student_age} ani.
Explică conceptul "{request.topic}" la nivel {difficulty_name}.

Răspunde STRICT în acest format JSON:
{{
    "explanation": "explicație clară și detaliată pas cu pas",
    "examples": ["exemplu 1", "exemplu 2", "exemplu 3"],
    "key_points": ["punct cheie 1", "punct cheie 2", "punct cheie 3"]
}}"""

    response = await client.chat.completions.create(
        model="gpt-4o-mini",
        messages=[{"role": "user", "content": prompt}],
        response_format={"type": "json_object"},
        temperature=0.7
    )

    import json
    data = json.loads(response.choices[0].message.content)

    return ExplanationResponse(
        topic=request.topic,
        explanation=data["explanation"],
        examples=data["examples"],
        key_points=data["key_points"],
        subject=request.subject,
        difficulty_level=request.difficulty_level
    )


async def generate_problems(request: ProblemRequest) -> ProblemResponse:
    subject_name = SUBJECT_NAMES[request.subject]
    difficulty_name = DIFFICULTY_NAMES[request.difficulty_level]

    prompt = f"""Ești un profesor expert în {subject_name} pentru elevi de {request.student_age} ani.
Generează {request.count} probleme despre "{request.topic}" la nivel {difficulty_name}.

Răspunde STRICT în acest format JSON:
{{
    "problems": ["problemă 1", "problemă 2"],
    "hints": ["indiciu pentru problema 1", "indiciu pentru problema 2"],
    "solutions": ["soluție completă 1", "soluție completă 2"]
}}"""

    response = await client.chat.completions.create(
        model="gpt-4o-mini",
        messages=[{"role": "user", "content": prompt}],
        response_format={"type": "json_object"},
        temperature=0.8
    )

    import json
    data = json.loads(response.choices[0].message.content)

    return ProblemResponse(
        topic=request.topic,
        problems=data["problems"],
        hints=data["hints"],
        solutions=data["solutions"],
        subject=request.subject,
        difficulty_level=request.difficulty_level
    )


async def get_hint(request: HintRequest) -> HintResponse:
    subject_name = SUBJECT_NAMES[request.subject]

    prompt = f"""Ești un profesor de {subject_name}.
Un elev are dificultăți cu această problemă: "{request.question}"
Oferă un indiciu util fără să dai răspunsul complet.

Răspunde STRICT în acest format JSON:
{{
    "hint": "indiciul tău aici"
}}"""

    response = await client.chat.completions.create(
        model="gpt-4o-mini",
        messages=[{"role": "user", "content": prompt}],
        response_format={"type": "json_object"},
        temperature=0.7
    )

    import json
    data = json.loads(response.choices[0].message.content)

    return HintResponse(
        question=request.question,
        hint=data["hint"],
        subject=request.subject
    )
