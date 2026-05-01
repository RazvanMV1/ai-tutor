from fastapi import APIRouter, HTTPException
from app.schemas.ai_schemas import ExplanationRequest, ExplanationResponse, HintRequest, HintResponse
from app.services.ai_service import get_explanation, get_hint
import traceback
import logging

logger = logging.getLogger("uvicorn.error")
router = APIRouter()


@router.post("", response_model=ExplanationResponse)
async def create_explanation(request: ExplanationRequest):
    try:
        return await get_explanation(request)
    except Exception as e:
        tb = traceback.format_exc()
        logger.error(f"Explanation error: {e}\n{tb}")
        raise HTTPException(status_code=500, detail=f"{type(e).__name__}: {str(e)}")


@router.post("/hint", response_model=HintResponse)
async def create_hint(request: HintRequest):
    try:
        return await get_hint(request)
    except Exception as e:
        tb = traceback.format_exc()
        logger.error(f"Hint error: {e}\n{tb}")
        raise HTTPException(status_code=500, detail=f"{type(e).__name__}: {str(e)}")
