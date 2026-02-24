from fastapi import APIRouter, HTTPException
from app.schemas.ai_schemas import ProblemRequest, ProblemResponse
from app.services.ai_service import generate_problems

router = APIRouter()


@router.post("", response_model=ProblemResponse)
async def create_problems(request: ProblemRequest):
    try:
        return await generate_problems(request)
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
