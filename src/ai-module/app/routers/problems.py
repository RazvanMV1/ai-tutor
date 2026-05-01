from fastapi import APIRouter, HTTPException
from app.schemas.ai_schemas import ProblemRequest, ProblemResponse
from app.services.ai_service import generate_problems
import traceback
import logging

logger = logging.getLogger("uvicorn.error")
router = APIRouter()


@router.post("", response_model=ProblemResponse)
async def create_problems(request: ProblemRequest):
    try:
        return await generate_problems(request)
    except Exception as e:
        tb = traceback.format_exc()
        logger.error(f"Problems error: {e}\n{tb}")
        raise HTTPException(status_code=500, detail=f"{type(e).__name__}: {str(e)}")
