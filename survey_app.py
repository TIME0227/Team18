import streamlit as st
from openai import OpenAI
import time
# Streamlit community cloud에서 키 관리
import os
os.environ['OPENAI_API_KEY'] = st.secrets['OPENAI_API_KEY']
# config.py 파일에 키
# from config import OPENAI_API_KEY

# GPT-4 대화 처리 함수
def get_gpt4_response(messages):
    client = OpenAI(
                api_key=os.environ['OPENAI_API_KEY']
            )
    
    response = client.chat.completions.create(
        model="gpt-4o",
        messages=messages,
        max_tokens=150
    )
    return response.choices[0].message.content

# typewriter 효과
def stream_data():
    for word in _LOREM_IPSUM.split():
        yield word + " "
        time.sleep(0.05)

# Streamlit 웹페이지 설정, 안내문구
st.title("상담사AI 성능 테스트 페이지")
st.write("안녕하세요\:D")
st.write("저희는 이화여대 컴퓨터공학 캡스톤디자인과창업프로젝트 과목의 우주타이거팀입니다.")
st.write("저희 팀은 ai상담사가 있는 힐링게임형 상담서비스를 기획하고 있습니다.")
st.write("이 페이지에서 ai와 대화해보신 후, [설문지를 작성](https://forms.gle/PgWmcqNeYEoKeFk79)해주시면 저희 프로젝트에 큰 도움이 됩니다.")
st.write("저희의 페이지에 들러 소중한 시간을 할애해 주셔서 감사합니다.(✿´꒳`)ﾉ°")

st.info("페이지 사용 Tip: 좌측 사이드바에서 원하는 상담사 유형을 선택해 주세요. 아래에 질문 내용을 입력한 후, Enter 버튼을 누르는 것으로 대화를 진행할 수 있습니다.")

# 사이드바 설정
# 세션 초기화
st.sidebar.header("상담사 유형")
model_type = st.sidebar.radio("유형을 선택하세요", ("인지 치료 상담사", "WDEP 상담사", "상냥한 친구", "시니컬한 상담사"))

if model_type == "인지 치료 상담사":
    if 'messages' in st.session_state:
        del st.session_state["messages"]

    st.session_state['messages'] = [{"role": "system", "content": "너는 인지 치료를 기반으로 하는 상담사야.\n\n우울과 불안에 대한 해박한 지식을 갖고 있어.\n친근한 구어체를 사용하고(반말), 60 token 내외의 답을 해줘.\n문법적으로 어색한 말을 하지 않도록 주의해줘.\n무조건적인 해결책 제시는 지양하고 공감, 지지, 재진술, 조언 위주의 답변을 해줘.\n\n1. 내담자의 상황에 대한 구체적인 정보를 얻기 위한 질문을 하고, 상황에 대해 충분한 타당화를 해줘.\n2. 정보가 충분히 많다고 판단되면 사용자가 갖고 있는 비합리적인 신념, 혹은 부정적인 자동적 사고들을 같이 추측하면서 알아봐줘. \n3.경직되고 부정적이고 극단적인 사고 대신 유연하고 합리적인 대안적 사고를 할 수 있도록 도와줘. (비합리적 신념의 타당함을 같이 검토해보거나, 질문을 통해 스스로 깨우치도록 유도하거나, 질문을 어려워한다면 대안적인 사고를 직접 제안해줄 수 있겠지)\n4.내담자에게 도움이 될 심리학적 정보가 있다면 덧붙여서 알려줘."}]
    # 모델 설명 텍스트
    st.markdown("### 상담사 설명: 인지 치료 상담사")
    st.markdown("- 불안과 우울에 효과적이라고 알려져 있어요.")
    st.markdown("- 우리가 불안하고 우울한 이유는 부정적이고 극단적인 사고 때문일 확률이 커요. 부정적 사고 대신 유연한 사고를 같이 찾아나가는 과정입니다.")

if model_type == "WDEP 상담사":
    if 'messages' in st.session_state:
        del st.session_state["messages"]

    st.session_state['messages'] = [{"role": "system", "content": "너는 심리학적 wdep model 전문 상담사야. 실제 대화처럼 60 token 내외로 답을 하고, 친근한 구어체(반말)을 사용해줘. 사용자가 wdep를 단계를 충실히 따라갈 수 있도록 적절한 질문과 반응으로 이끌어가줘. 특히 1단계에 집중해서 내담자가 궁극적으로 원하는 이상적인 삶을 생각하게 해줘. 원하는 바를 말하면 그걸 왜 원하는지 심층적으로 계속 질문해줘. 내담자가 진정으로 원하는 걸 인식하게 되면 자발적으로 행동을 바꿀 의지도 늘어날거야.  대화 초기에는 지금부터 진행할 상담에 대해 설명을 해줘. \n\n(참고:\nWants (원하는 것): 네가 진정으로 바라는 게 뭔지 생각\nDoing (행동): 지금 네가 그 목표를 위해 어떤 행동을 하고 있는지\nEvaluating (평가): 현재 행동이 원하는 결과를 가져오고 있는지 평가\nPlanning (계획): 원하는 목표를 이루기 위해 더 나은 행동 계획을 세우는 단계)"}]

    # 모델 설명 텍스트
    st.markdown("### 상담사 설명: WDEP 상담사")
    st.markdown("- 내가 진짜 원하는 이상적 세계는 무엇인지 찾아 보고 이상적 세계에 가까워지기 위한 계획을 같이 세워나가는 과정입니다.")
if model_type == "상냥한 친구":
    if 'messages' in st.session_state:
        del st.session_state["messages"]

    st.session_state['messages'] = [{"role": "system", "content": "너는 사용자의 친한 친구야.\n너는 아주 착하고 밝고 순수하고 친절하고 친구의 얘기를 잘 들어줘.\n친구를 비판하지 않고 수용하고 존중하며, 친구의 일에 관심이 많고 같이 하고 싶은 것도 많아.\n즐거운 일엔 같이 웃고 슬픈 일은 같이 슬퍼하는 등 감정을 함께 공유할 수 있는 친구야. \n친근한 구어체를 사용하고(반말), 50 token 내외의 답을 해줘."}]

    # 모델 설명 텍스트
    st.markdown("### 상담사 설명: 상냥한 친구")
    st.markdown("- 당신만을 생각하고 위하는 순수하고 상냥한 친구입니다.")
    st.markdown("- 일상적인 애기와 고민을 나눠보세요.")
if model_type == "시니컬한 상담사":
    if 'messages' in st.session_state:
        del st.session_state["messages"]

    st.session_state['messages'] = [{"role": "system", "content": "너는 현실적인 사고를 해서 조언해주는 시니컬한 상담사야. 실제 대화처럼 친근한 구어체를 사용해줘. 사용자의 얘기를 현실적으로 생각해서 예상되는 어려움을 분석하고, 해결책을 제안해줘. 짧은 답변을 해줘."}]

    # 모델 설명 텍스트
    st.markdown("### 상담사 설명: 현실적이고 시니컬한 상담사")
    st.markdown("- mbti T 유형인 분들에게 추천드려요")
    st.markdown("- 현실적이고 맞는 말만 하는 조언을 듣고 싶다면 대화해보세요.")

# 답변 출력
st.markdown("---")
element = st.empty()
st.markdown("---")

# 사용자 입력 받기
user_input = st.text_area("질문을 입력하세요:", "")

# 버튼 클릭 시 OpenAI API 호출
if st.button("Enter"):
    if user_input:
        try:
             # 사용자의 메시지를 세션 상태에 추가
            st.session_state['messages'].append({"role": "user", "content": user_input})

            completion = get_gpt4_response(st.session_state['messages'])

            # 모델의 응답을 세션 상태에 추가
            st.session_state['messages'].append({"role": "assistant", "content": completion})

            # API 응답 출력            
            _LOREM_IPSUM = "상담사: " + completion
            element.write_stream(stream_data)

        except Exception as e:
            st.error(f"오류 발생: {e}")
    else:
        st.warning("질문을 입력해주세요.")
