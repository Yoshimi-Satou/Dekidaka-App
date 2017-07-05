using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlatformInvoke
{

    /// <summary>
    /// 保留されているウィンドウメッセージを削除
    /// </summary>
    public static class RemovePeekMessage
    {
        #region 定数
        /// <summary>
        /// 該当するウィンドウに再描画が必要とOSが判断したときにポストされるメッセージ
        /// </summary>
        private const int WM_PAINT = 0xF;

        /// <summary>
        ///  ウィンドウのクライアント領域でユーザーがマウスの移動をしたときにポストされるメッセージ
        /// </summary>
        private const int WM_MOUSEMOVE = 0x200;

        /// <summary>
        /// ウィンドウのクライアント領域でユーザーがマウスの左ボタンを押したときにポストされるメッセージ
        /// </summary>
        private const int WM_LBUTTONDOWN = 0x201;

        /// <summary>
        /// ウィンドウのクライアント領域でユーザーがマウスの左ボタンを離したときにポストされるメッセージ
        /// </summary>
        private const int WM_LBUTTONUP = 0x202;

        /// <summary>
        /// マウスポインタが領域外に出た時にポストされるメッセージ
        /// </summary>
        private const int WM_MOUSELEAVE = 0x2A3;

        #endregion

        #region 構造体
        /// <summary>
        /// MSG 構造体型
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            /// <summary>
            /// ウィンドウハンドル
            /// </summary>
            public IntPtr hWnd;
            /// <summary>
            /// MessageID
            /// </summary>
            public int msg;
            /// <summary>
            /// WParamフィールド（MessageIDごとに違うよ)
            /// </summary>
            public IntPtr wParam;
            /// <summary>
            /// LParamフィールド（MessageIDごとに違うよ)
            /// </summary>
            public IntPtr lParam;
            /// <summary>
            /// 時間
            /// </summary>
            public int time;
            /// <summary>
            /// カーソル位置（スクリーン座標）
            /// </summary>
            public POINTAPI pt;
        }

        /// <summary>
        /// POINTAPI構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTAPI
        {
            /// <summary>
            /// X座標
            /// </summary>
            public int x;
            /// <summary>
            /// Y座標
            /// </summary>
            public int y;
        }
        #endregion

        #region 列挙型
        /// <summary>
        /// wRemoveMsgで利用可能なオプション
        /// </summary>
        enum EPeekMessageOption
        {
            /// <summary>
            /// 処理後メッセージをキューから削除しない
            /// </summary>
            PM_NOREMOVE = 0,
            /// <summary>
            /// 処理後メッセージをキューから削除する
            /// </summary>
            PM_REMOVE
        }
        #endregion

        #region DllImport
        /// <summary>
        /// スレッドのメッセージキューにメッセージがあるかどうかをチェックし
        /// あれば、指定された構造体にそのメッセージを格納します
        /// </summary>
        /// <param name="lpMsg"> メッセージ情報を格納する、MSG 構造体型変数のポインタを指定します</param>
        /// <param name="hWnd">メッセージを取得するウィンドウのハンドルを指定します。全ての場合は NULL</param>
        /// <param name="wMsgFilterMin">メッセージの最小値を指定し、フィルタリングします。しない場合は0</param>
        /// <param name="wMsgFilterMax">メッセージの最大値を指定し、フィルタリングします。しない場合は0</param>
        /// <param name="wRemoveMsg">処理後メッセージをキューを削除するか否か</param>
        /// <returns>メッセージを取得したときは 0 以外、取得しなかったときは 0</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PeekMessage(
            out MSG lpMsg,
            int hWnd,
            int wMsgFilterMin,
            int wMsgFilterMax,
            EPeekMessageOption wRemoveMsg
        );

        /// <summary>
        /// 指定された MSG 構造体型変数にメッセージを格納します
        /// </summary>
        /// <param name="lpMsg">メッセージ情報を格納する、MSG 構造体型変数のポインタを指定します</param>
        /// <param name="hWnd">メッセージを取得するウィンドウのハンドルを指定します。全ての場合は NULL</param>
        /// <param name="wMsgFilterMin">メッセージの最小値を指定し、フィルタリングします。しない場合は0</param>
        /// <param name="wMsgFilterMax">メッセージの最大値を指定し、フィルタリングします。しない場合は0</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMessage(
            out MSG lpMsg,
            int hWnd,
            int wMsgFilterMin,
            int wMsgFilterMax
        );

        /// <summary>
        /// 指定されたウィンドウメッセージをウィンドウプロシージャにディスパッチします
        /// </summary>
        /// <param name="lpMsg">ディスパッチするメッセージを格納した MSG 構造体変数のポインタを指定します</param>
        /// <returns>ウィンドウプロシージャの戻り値</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr DispatchMessage(
             out MSG lpMsg
        );
        #endregion

        #region メソッド
        /// <summary>
        /// 保留されているウィンドウメッセージを削除します。
        /// </summary>
        public static void Invoke()
        {
            MSG wm;
            while (PeekMessage(out wm, 0, 0, 0, EPeekMessageOption.PM_REMOVE))
            {
                switch (wm.msg)
                {
                    case WM_LBUTTONUP:
                    case WM_MOUSELEAVE:
                    case WM_PAINT:
                        DispatchMessage(out wm);
                        break;
                }
            }
        }
        #endregion
    }
}