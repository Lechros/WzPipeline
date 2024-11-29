using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Soul;

namespace WzJson.Tests;

public class SoulResourceTests : IClassFixture<WzProviderFixture>
{
    private WzProviderFixture wzProviderFixture;

    public SoulResourceTests(WzProviderFixture wzProviderFixture)
    {
        this.wzProviderFixture = wzProviderFixture;
    }

    private IWzProvider WzProvider => wzProviderFixture.WzProvider;

    [Theory]
    [InlineData("림보", "근원의 림보")]
    [InlineData("카링", "유폐")]
    [InlineData("칼로스", "감시자의 포효")]
    [InlineData("듄켈", "지면 절단")]
    [InlineData("진 힐라", "영혼 찢기")]
    [InlineData("윌", "파괴의 손아귀")]
    [InlineData("루시드", "악몽으로의 초대")]
    [InlineData("데미안", "사냥 개시")]
    [InlineData("스우", "때렸스우~")]
    [InlineData("섬멸병기 스우", "기계팔 돌진")]
    [InlineData("매그너스", "진격! 그게 바로 나다")]
    [InlineData("시그너스", "불꽃여제")]
    [InlineData("블러디퀸", "여왕의 마음은 갈대")]
    [InlineData("벨룸", "기가 벨룸 레이저")]
    [InlineData("무르무르", "무르무르의 이상한 동행")]
    [InlineData("핑크빈", "까칠한 귀여움")]
    [InlineData("반반", "불닭의 따끔한 맛")]
    [InlineData("피에르", "피에르의 모자선물")]
    [InlineData("우르스", "파왕의 포효")]
    [InlineData("아카이럼", "스네이크 사우론")]
    [InlineData("모카딘", "검은 기사 모카딘")]
    [InlineData("카리아인", "미친 마법사 카리아인")]
    [InlineData("줄라이", "인간 사냥꾼 줄라이")]
    [InlineData("CQ57", "돌격형 CQ57")]
    [InlineData("플레드", "싸움꾼 플레드")]
    [InlineData("반 레온", "야옹이 권법 : 할퀴기 초식")]
    [InlineData("힐라", "마른 하늘에 번개 어택")]
    [InlineData("파풀라투스", "공간의 지배자")]
    [InlineData("자쿰", "뜨거운 토템 투하")]
    [InlineData("발록", "지옥불 트림")]
    [InlineData("돼지바", "돼지바 스윙!")]
    [InlineData("프리미엄PC방", "PC방에서 메이플을 켰다!")]
    [InlineData("무공", "회춘신공")]
    [InlineData("피아누스", "공포의 마빡생선")]
    [InlineData("렉스", "내 앞길을 막지마")]
    [InlineData("드래곤 라이더", "손바닥 장풍")]
    [InlineData("에피네아", "여왕의 향기는 나빌레라")]
    [InlineData("핑크몽", "해피 뉴 에브리데이!")]
    [InlineData("교도관 아니", "난 한 놈만 패")]
    [InlineData("락 스피릿", "로큰롤 베이비")]
    [InlineData("캡틴 블랙 슬라임", "핑크빛 독안개")]
    [InlineData("크세르크세스", "특공 염소 어택")]
    public void KnownNormalSkillId_WithWz_EqualsExpectedName(string mobName, string expectedName)
    {
        var skillId = SoulResource.KnownNormalSkillIds[mobName];
        var skillNode = WzProvider.BaseNode.FindNodeByPath(GetSkillNameNodePath(skillId), true);
        var skillName = skillNode?.GetValue<string>();
        skillNode?.GetNodeWzImage()?.Unextract();

        Assert.Equal(expectedName, skillName);
    }

    [Theory]
    [InlineData("림보", "솟구치는 그림자")]
    [InlineData("카링", "수라")]
    [InlineData("칼로스", "침입자 처단")]
    [InlineData("듄켈", "지면 파쇄")]
    [InlineData("진 힐라", "붉은 마녀")]
    [InlineData("윌", "거미의 왕")]
    [InlineData("루시드", "악몽의 지배자")]
    [InlineData("데미안", "파멸의 검")]
    [InlineData("스우", "화났스우~")]
    [InlineData("섬멸병기 스우", "어둠의 기운")]
    [InlineData("매그너스", "폭격! 그게 바로 나다")]
    [InlineData("시그너스", "폭풍여제")]
    [InlineData("블러디퀸", "여왕님이 함께 하셔!")]
    [InlineData("벨룸", "주니어 벨룸 소환!")]
    [InlineData("무르무르", "무르무르의 수상한 동행")]
    [InlineData("핑크빈", "치명적인 귀여움")]
    [InlineData("반반", "치킨 날다!")]
    [InlineData("피에르", "깜짝 피에르!")]
    [InlineData("우르스", "파왕의 거친 포효")]
    [InlineData("아카이럼", "메두사카이럼")]
    [InlineData("모카딘", "어둠 기사 모카딘")]
    [InlineData("카리아인", "폭주 마법사 카리아인")]
    [InlineData("줄라이", "피의 사냥꾼 줄라이")]
    [InlineData("CQ57", "상급 돌격형 CQ57")]
    [InlineData("플레드", "거친 싸움꾼 플레드")]
    [InlineData("반 레온", "야옹이 권법 : 크로스 따귀 어택")]
    [InlineData("힐라", "마른 하늘에 벼락 어택")]
    [InlineData("파풀라투스", "시간의 지배자")]
    [InlineData("자쿰", "화끈한 토템 투하")]
    [InlineData("발록", "지옥불 재채기")]
    [InlineData("돼지바", "돼지바 드롭!")]
    [InlineData("프리미엄PC방", "프리미엄PC방은 빵빵해")]
    public void KnownMagnificentSkillId_WithWz_EqualsExpectedName(string mobName, string expectedName)
    {
        var skillId = SoulResource.KnownMagnificentSkillIds[mobName];
        var skillNode = WzProvider.BaseNode.FindNodeByPath(GetSkillNameNodePath(skillId), true);
        var skillName = skillNode?.GetValue<string>();
        skillNode?.GetNodeWzImage()?.Unextract();

        Assert.Equal(expectedName, skillName);
    }

    private string GetSkillNameNodePath(int skillId)
    {
        return @$"String\Skill.img\{skillId}\name";
    }
}