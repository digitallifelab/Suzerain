﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using GoogleMobileAds.Api;
using Soomla.Store;

public class WaitOpponentDialog : MonoBehaviour {

	public GameObject 	fightRequestPanelObject;
	public Text			actionDescription;
	public Button 		cancelButton;
	public Text			header;

	private WaitOpponentDialog waitOpponentPanel;
	BannerView bannerView = null;

	string[] suggestionArrayRus = {
		"\"Знание есть сила, сила есть знание\"\n-Френсис Бэкон",
		"\"Знания, не рожденные опытом, бесплодны и полны ошибок\"\n-Леонардо да Винчи",
		"\"Не бойся незнания, бойся ложного знания\"\n-Лев Толстой",
		"\"Чтобы переваривать знания, надо поглощать их с аппетитом\"\n-Анатоль Франс",
		"\"Блаженство тела состоит в здоровье, блаженство ума – в знании\"\n-Фалес"
		,"\"Знание есть сокровище, но хранитель его – разум\"\n - Пенн Вильям"
		,"\"Чем меньше люди знают, тем обширнее кажется им их знание\"\n-Жан-Жак Руссо"
		,"\"Как приятно знать, что ты что-то узнал!\"\n-Мольер"
		,"\"Знающий меру доволен своим положением. Знающий много – молчалив, а говорящий много не знает ничего\"\n-Лао-Цзы"
		,"\"Язык, который умудрен знаниями, не будет запинаться\"\n-Менандр"
		,"\"Недостаточно только получить знания; надо найти им приложение. Недостаточно только желать; надо делать\"\n-Иоганн Вольфганг фон Гёте"
		,"\"Кто думает, что постиг все, тот ничего не знает\"\n-Лао-Цзы"
		,"\"Есть одно только благо – знание и одно только зло – невежество\"\n Сократ"
		,"\"Я знаю только то, что ничего не знаю\"\n - Сократ"
		,"\"Интеллектуально независимы – только гении и дураки\"\n - Станислав Ежи Лец"
		,"\"Ум подобен здоровью: тот, кто им обладает, его не замечает\"\n - Клод Гельвеций"
		,"\"Ум выше храбрости\"\n - Федр"
		,"\"Не бывало великого ума без примеси безумия\"\n - Сенека"
		,"\"Ум заключается не только в знании, но и в умении прилагать знание на деле\"\n - Аристотель"
		,"\"Ум - это не сосуд, который надо заполнить, а факел, который необходимо зажечь\"\n - Плутарх"
		,"\"Очень полезно оттачивать и шлифовать свой ум об умы других\"\n - Мишель Монтень"
		,"\"Умный многому сумеет научиться у противника\"\n - Аристофан"
		,"\"Всем умным людям следует находиться во взаимном общении\"\n - Плавт"
		,"\"Силу уму придают упражнения, а не покой\"\n - Александр Поуп"
		,"\"Мало иметь хороший ум, главное – хорошо его применять\"\n - Декарт"
		,"\"Высшая мудрость – знать самого себя\"\n - Галилео Галилей"
		,"\"Мудрец избегает всякой крайности\"\n - Лао-Цзы"
		,"\"Нет большей мудрости, чем своевременность\"\n - Фрэнсис Бэкон"
		,"\"Мудр тот, кто знает не многое, а нужное\"\n - Эсхил"
		,"\"Настоящий признак, по которому можно узнать истинного мудреца – терпение\"\n - Генрик Ибсен"
		,"\"Кто мудр, тот и добр\"\n - Сократ"
		,"\"Мудрец сам творит свою судьбу\"\n - Плавт"
		,"\"Опыт увеличивает нашу мудрость, но не уменьшает нашей глупости\"\n - Генри Уиллер Шоу"
		,"\"Сомнение – полпути к мудрости\"\n - Публилий Сир"
		,"\"Старайся прежде быть мудрым, а ученым, – когда будешь иметь свободное время\"\n - Пифагор"
		,"\"Торжество над самим собой есть венец философии\"\n - Диоген"
		,"\"Упущенный случай редко повторяется\"\n-Публий"
		,"\"Кто время выиграл – все выиграл в итоге\"\n - Мольер"
		,"\"Все приходит в свое время для тех, кто умеет ждать\"\n - Оноре де Бальзак"
		,"\"Истинно велик тот человек, который сумел овладеть своим временем!\"\n -Гесиод"
		,"\"Мудрее всего – время, ибо оно раскрывает все\"\n- Фалес"
		,"\"Раз начатое не может быть остановлено\"\n- Цицерон"
		,"\"Удача – это постоянная готовность использовать шанс\"\n- Фрэнк Доуби"
		,"\"Удача не бродит где-то вдалеке, удача прячется в тебе самом\"\n- Теткоракс"
		,"\"Если человек не верит в удачу, у него небогатый жизненный опыт\"\n- Джозеф Конрад"
		,"\"Удачная возможность прячется в трудностях\"\n- Альберт Эйнштейн"
		,"\"Удача выбирает того, кто к ней готов\"\n- Луи Пастер"
		,"\"Запоминать умеет тот, что умеет быть внимательным\"\n- Сэмюэл Джонсон"
		,"\"Память слабеет, если ее не упражняешь\"\n- Цицерон"
		,"\"Верная и деятельная память удваивает жизнь\"\n- Мирабо"
		,"\"Интуиция никогда не подводит того, кто ко всему готов\"\n- Иммануил Кант"
		,"\"Есть только два способа прожить жизнь. Первый – будто чудес не существует. Второй – будто кругом одни чудеса.\"\n- Альберт Эйнштейн"
		,"\"Образование – это то, что остаётся после того, как забывается всё выученное в школе\"\n- Альберт Эйнштейн"
		,"\"Только те, кто предпринимают абсурдные попытки, смогут достичь невозможного\"\n - Альберт Эйнштейн"
		,"\"Бессмысленно продолжать делать то же самое и ждать других результатов\"\n - Альберт Эйнштейн"
		,"\"Ты никогда не решишь проблему, если будешь думать так же, как те, кто ее создал\"\n- Альберт Эйнштейн"
		,"\"Жизнь – как вождение велосипеда. Чтобы сохранить равновесие, ты должен двигаться\"\n- Альберт Эйнштейн"
		,"\"Разум, однажды расширивший свои границы, никогда не вернется в прежние\"\n - Альберт Эйнштейн"
		,"\"Человек начинает жить лишь тогда, когда ему удается превзойти самого себя\"\n- Альберт Эйнштейн"
		,"\"Стремись не к тому, чтобы добиться успеха, а к тому, чтобы твоя жизнь имела смысл\"\n- Альберт Эйнштейн"
		,"\"При помощи совпадений Бог сохраняет анонимность\"\n- Альберт Эйнштейн"
		,"\"Самое непостижимое в этом мире – это то, что он постижим\"\n- Альберт Эйнштейн"
		,"\"Человек, никогда не совершавший ошибок, никогда не пробовал ничего нового\"\n - Альберт Эйнштейн"
		,"\"Вы думаете, всё так просто? Да, всё просто. Но совсем не так\"\n - Альберт Эйнштейн"
		,"\"Логика может привести Вас от пункта А к пункту Б, а воображение – куда угодно...\"\n- Альберт Эйнштейн"
		,"\"Чтобы выигрывать, прежде всего нужно играть\"\n - Альберт Эйнштейн"
		,"\"Любой кризис – это новые возможности\"\n - Уинстон Черчилль"
		,"\"Умный человек не делает сам все ошибки – он дает шанс и другим\"\n - Уинстон Черчилль"
		,"\"Успех – это способность шагать от одной неудачи к другой, не теряя энтузиазма\"\n - Уинстон Черчилль"
		,"\"Сокол высоко поднимается, когда летит против ветра, а не по ветру\"\n - Уинстон Черчилль"
		,"\"Глуп тот человек, который никогда не меняет своего мнения\"\n - Уинстон Черчилль"
		,"\"Ложь успевает обойти полмира, пока правда надевает штаны\"\n - Уинстон Черчилль"
		,"\"Люди прекрасно умеют хранить секреты, которых не знают\"\n - Уинстон Черчилль"
		,"\"Величайший урок жизни в том, что и дураки бывают правы\"\n - Уинстон Черчилль"
		,"\"Ничем так не завоюешь авторитета, как спокойствием\"\n - Уинстон Черчилль"
		,"\"Пессимист видит трудность в каждой возможности; оптимист видит возможность в каждой трудности\"\n- Уинстон Черчилль"
		,"\"Даже самого ослепительного света не бывает без тени\"\n - Уинстон Черчилль"
		,"\"На самом деле, жизнь проста, но мы настойчиво её усложняем\"\n - Конфуций"
		,"\"Советы мы принимаем каплями, зато раздаём ведрами\"\n - Конфуций"
		,"\"Можно всю жизнь проклинать темноту, а можно зажечь маленькую свечку\"\n - Конфуций"
		,"\"Красота есть во всем, но не всем дано это видеть\"\n-Конфуций"
		,"\"Благородный муж в душе безмятежен. Низкий человек всегда озабочен.\"\n - Конфуций"
		,"\"Не тот велик, кто никогда не падал, а тот велик – кто падал и вставал\"\n - Конфуций"
		,"\"Главное – мудрость: приобретай мудрость и всем имением твоим приобретай разум\"\n- Соломон"
		,"\"Кто весь день работает, тому некогда зарабатывать деньги\"\n- Дэвид Рокфеллер"
		,"\"Ваше благополучие зависит от ваших собственных решений\"\n- Дэвид Рокфеллер"
		,"\"Знаете, что доставляет самое большое удовольствие? Видеть, как приходят дивиденды от вложенных усилий\"\n- Дэвид Рокфеллер"
		,"\"Благодаря упорству, все, что угодно, — будь то правильное или нет, хорошее или плохое, — будет достигнуто\"\n- Дэвид Рокфеллер"
		,"\"Никто никогда не догадывается, кем окажется в этой жизни, но всегда надо знать, что рожден для чего-то большего\"\n- Дэвид Рокфеллер"
		,"\"Не бойтесь отказываться от хорошего в пользу отличного\"\n- Дэвид Рокфеллер"
		,"\"Заработайте репутацию, и она будет работать на вас\"\n- Дэвид Рокфеллер"
		,"\"Нет другого качества, столь необходимого для успеха любого рода, как настойчивость. Настойчивость может преодолеть все, даже законы природы\"\n- Дэвид Рокфеллер"
		,"\"Уметь выносить одиночество и получать от него удовольствие — великий дар\"\n- Бернард Шоу"
		,"\"Самый большой грех по отношению к ближнему — не ненависть, а равнодушие; вот истинно вершина бесчеловечности\"\n- Бернард Шоу"
		,"\"Тот, кто умеет, тот делает, кто не умеет — тот учит других\"\n- Бернард Шоу"
		,"\"Постарайтесь получить то, что любите, иначе придется полюбить то, что получили\"\n- Бернард Шоу"
		,"\"Природа не терпит пустоты: там, где люди не знают правды, они заполняют пробелы домыслом\"\n- Бернард Шоу"
		,"\"Вы видите вещи, и вы спрашиваете: «Почему?» А я мечтаю о вещах, которых никогда не было, и говорю: «Почему бы и нет?»\"\n- Бернард Шоу"
		,"\"Смиренно склони голову перед фактами, но гордо подними ее перед лицом чужих мнений\"\n- Бернард Шоу"
		,"\"Никогда не поздно уйти из толпы. Следуй за своей мечтой, двигайся к своей цели\"\n- Бернард Шоу"
		,"\"Жизнь не научит, если нет желания поумнеть\"\n- Бернард Шоу"
		,"\"Жизнь, счастливая или несчастливая, удачная или неудачная, все же исключительно интересна\"\n- Бернард Шоу"
		,"\"Мудрость не всегда приходит с возрастом. Бывает, что возраст приходит один\"\n- Михаил Жванецкий"
		,"\"Если ты споришь с идиотом, то, вероятно, то же самое делает и он\"\n- Михаил Жванецкий"
		,"\"Я же говорил: «Или я буду жить хорошо, или мои произведения станут бессмертными». И жизнь опять повернулась в сторону произведений\"\n- Михаил Жванецкий"
		,"\"Добpо всегда побеждает зло, значит, кто победил, тот и добрый\"\n- Михаил Жванецкий"
		,"\"Труднее всего человеку дается то, что дается не ему\"\n- Михаил Жванецкий"
		,"\"Разница между умным и мудрым: умный с большим трудом выкручивается из ситуации, в которую мудрый не попадает\"\n- Михаил Жванецкий"
		,"\"Мыслить так трудно, поэтому большинство людей судит\"\n- Михаил Жванецкий"
		,"\"У одних оба полушария защищены черепом, у других — штанами\"\n- Михаил Жванецкий"
		,"\"Дело не в пессимизме и не в оптимизме, а в том, что у девяноста девяти из ста нет ума\"\n- Антон Чехов"
		,"\"Всё знают и всё понимают только дураки да шарлатаны\"\n- Антон Чехов"
		,"\"Если твой поступок огорчает кого-нибудь, то это еще не значит, что он дурен\"\n- Антон Чехов"
		,"\"Жизнь, по сути, очень простая штука и человеку нужно приложить много усилий, чтобы её испортить\"\n- Антон Чехов"
		,"\"Никто не хочет любить в нас обыкновенного человека\"\n- Антон Чехов"
		,"\"Там, где есть жизнь, есть надежда\"\n- Стивен Хокинг"
		,"\"Убежден, что наука и исследовательская деятельность приносят больше удовольствия, чем зарабатывание денег\"\n- Стивен Хокинг"
		,"\"Главный враг знания не невежество, а иллюзия знания\"\n- Стивен Хокинг"
		,"\"Среди всех систем, которые у нас есть, самые сложные — это наши собственные тела\"\n- Стивен Хокинг"
		,"\"Жизнь была бы очень трагичной, если бы не была такой забавной\"\n- Стивен Хокинг"
		,"\"Очень важно просто не сдаваться\"\n- Стивен Хокинг"
		};

	string[] suggestionArrayEng = {
		"Каждый день, при входе в игру, Вам начисляется рента в пистолях, соответствующая Вашему титулу.",
		"Каждый участник дуэли ставит на кон одинаковое количество пистолей. Победитель получает все.",
		"Дуэльный клуб позволяет проводить поединки не только на пистолетах - Ваши знания и удача - это тоже оружие.",
		"Разные виды дуэлей подразумевают разную величину ставок при их проведении.",
		"Количество проведенных поединков и одержанных побед - основа для продвижения по иерархии титулов.",
		"Чем выше Ваш титул, тем значительнее сумма пистолей, которую Вы получаете каждый день при входе в игру.",
		"Станьте одним из Принцев - и Вы сможете претендовать на титул Сюзерена - высший статус в Дуэльном клубе.",
		"Станьте Сюзереном и войдите в историю Дуэльного клуба, чеканя монеты со своим гербом.",
		"Имя нового Сюзерена - первого среди Принцев - провозглашается один раз в тринадцать дней.",
		"Не забывайте посещать гардероб Дуэльного клуба - время от времени там появляются совершенно уникальные предметы.",
		"Желаете пошить дуэльный костюм на заказ? Нет ничего невозможного. Пишите нашим портным!",
		"Некоторые предметы, которые Вы сможете приобрести в гардеробе Дуэльного клуба, дадут Вам доступ к Тайным миссиям.",
		"Успешное выполнение Тайных миссий Дуэльного клуба - залог получения уникальных впечатлений и наград.",
		"В гардеробе Дуэльного клуба Вы можете подобрать себе костюм, который произведет неизгладимое впечатление на Ваших противников.",
		"Отнеситесь с должным вниманием к подбору дуэльного пистолета - каждый из них обладает своими особенностями.",
		"Больше читайте. Ваши знания - это тоже оружие, которое позволит Вам одержать много побед. И не только в Дуэльном клубе.",
		"В Дуэльном клубе Вы сможете испытать не только свою точность, ловкость и знания, но и удачу. Куда же без нее?",
		"Обратите внимание - потратив пять пистолей Вы можете убрать два неверных ответа в поединке на знания. Но успеете ли?",
		"Знание правильного ответа - еще не гарантия победы в поединке умов. Скорость выбора тоже играет роль.",
		"Выдержка - немаловажный фактор в поединке на пистолетах. Точность, правда, тоже.",
		"Чем больше поединков - тем больше побед - тем выше титул - тем больше пистолей - тем больше возможностей. Тем интереснее, одним словом.",
		"В Кабинете, Вы всегда можете узнать свое место в глобальном рейтинге Дуэльного клуба. И в рейтинге Вашего города тоже.",
		"Обзаводитесь друзьями в Дуэльном клубе. Это залог увлекательных состязаний и безопасного сведения личных счетов)",
		"Результаты последних дуэлей Вы всегда сможете посмотреть в своем Кабинете.",
		"Регалии, которые Вы завоевали на поединках, хранятся в Вашем Кабинете.",
		"Каждому титулу соответсвует своя корона, которая отражается над Вашим гербом.",
		"Дуэльный клуб работает над созданием фехтовального зала. Вы любите холодное оружие?",
		"Интуиция, как мы полагаем, - немаловажный фактор успеха. Ожидайте в скором времени новых видов поединков."};

	public void SetText(string text, UnityAction cancelEvent){

		//if (!Utility.StopCoroutine) {
			if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) == 0) {
				RequestBanner ();
			}
		//}

		SoundManager.ChoosePlayMusic (Constants.SoundWaiting);

		fightRequestPanelObject.SetActive (true);

		if (UserController.currentUser.Language == "English") {
			actionDescription.text = suggestionArrayEng [Random.Range (0, suggestionArrayEng.Length - 1)];
		} else {
			actionDescription.text = System.Text.RegularExpressions.Regex.Unescape(suggestionArrayRus [Random.Range (0, suggestionArrayRus.Length - 1)]);
		}

		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener (cancelEvent);
		cancelButton.onClick.AddListener (ClosePanel);

		cancelButton.gameObject.SetActive (true);

		Text ttt = cancelButton.GetComponentInChildren (typeof(Text)) as Text;
		ttt.text = ScreensManager.LMan.getString ("@cancel");
		header.text = ScreensManager.LMan.getString ("@tofight");
	}
	
	public void ClosePanel () {
		fightRequestPanelObject.SetActive (false);

		if (bannerView != null) {
			bannerView.Hide ();
			bannerView.Destroy();
		}
		//SoundManager.ChoosePlayMusic (0);
	}

	private void RequestBanner()
	{
		#if UNITY_ANDROID
		string adUnitId = Constants.BANNER_ID_KEY_ANDROID;
		#elif UNITY_IPHONE
		string adUnitId = Constants.BANNER_ID_KEY_IOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif


		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Top);
//		bannerView.OnAdLoaded += (object sender, System.EventArgs e) => {
//			Debug.Log("ВОТ ОНО");
//			bannerView.Show ();
//		};

		bannerView.OnAdLoaded += HandleAdLoaded;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
		.TagForChildDirectedTreatment(true)
		.Build();

//		AdRequest request = new AdRequest.Builder()
//		.AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
//		.AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")  // My test device.
//		.Build();

		// Load the banner with the request.
		bannerView.LoadAd(request);
		//bannerView.Show ();
	}

	public void HandleAdLoaded(object sender, System.EventArgs args) {
		bannerView.Show ();
	}

}