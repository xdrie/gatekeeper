const { h1, p } = Moon.view.m;

const view = ({ data }) =>
    <p>{ data.message }</p>

Moon.use({
    data: Moon.data.driver,
    view: Moon.view.driver("#view")
});

Moon.run(() => {
    const data = {
        message: "hello, world!"
    };

    return {
        data,
        view: <view data=data />
    };
});