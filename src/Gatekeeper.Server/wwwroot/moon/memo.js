const { div, p, form, label, input } = Moon.view.m;

const addMemo = ({ data }) => {
	const dataNew = {
        ...data,
		memo: ""
	};

	return {
		data: dataNew,
		view: <viewMemo data=dataNew/>
	};
};

const viewMemo = ({ data }) => (
    <div class="form-group">
        <label class="form-label" for="memo">enter a memo</label>
        <input type="text" class="form-input" id="memo" placeholder="type anything"
            value=data.memo />
    </div>
);

Moon.use({
            data: Moon.data.driver,
    view: Moon.view.driver("#view")
});

Moon.run(() => {
    const data = {
            memo: ""
    };

    return {
            data,
            view: <viewMemo data=data />
    };
});